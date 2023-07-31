using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSingleton : MonoBehaviour
{
    //public NetworkUser localUser;
    public GameObject playerPrefab;
    public PlayerRepo localRepo = new PlayerRepo();
    public uint localFrameID = 0;

    public void BatchTick(List<FrameOperation> buf)
    {
        //Debug.Log($"batch tick cnt:{buf.Count}, localframe:{localFrameID}");
        // 特判JOIN和EXIT两个操作
        foreach (var fopr in buf)
        {
            while (localFrameID < fopr.frameID)
            {
                localRepo.TickAllPlayer(Time.fixedDeltaTime);
                ++localFrameID;
            }
            //Debug.Log("fopr:" + fopr.keyset);
            if ((fopr.keyset & BaseController.GetBit(KeyType.JOIN)) > 0)
            {
                var pd = localRepo.RegisterPlayer(playerPrefab, fopr.netID, fopr.deviceID);
                //Debug.Log("after reg player cnt:" + localRepo.players.Count);
                //Debug.Log("REG ARG:" + fopr.netID + " " + fopr.deviceID);

            }
            else if ((fopr.keyset & BaseController.GetBit(KeyType.EXIT)) > 0)
                localRepo.DestroyPlayer(fopr.netID, fopr.deviceID);
            else if ((fopr.keyset & BaseController.GetBit(KeyType.EMPTY_FRAME)) > 0)
                continue;
            else
            {
                var pd = localRepo.GetPlayerDict(fopr.netID, fopr.deviceID);
                //Debug.Log("player cnt:" + localRepo.players.Count + pd.prefab);
                //Debug.Log("CTRL ARG:" + fopr.netID + " " + fopr.deviceID);
                var physical_ctrl = pd.prefab.GetComponent<PlayerPhysicalController>();
                //Debug.Log("player physical_ctrl:" + physical_ctrl);
                physical_ctrl.ctrl.keyset = fopr.keyset;
                physical_ctrl.ctrl.SetHorizon(fopr.horizontal);
            }

        }
    }

    public void InitAllPlayer(List<FrameOperation> history)
    {
        BatchTick(history);
    }

    public void CmdPushOperation(Operation _opr)
    {
        NetworkClient.Send<RequestPushOperation>(new() { opr = _opr });
    }
    public void CmdJoinPlayer(string _deviceID)
    {
        NetworkClient.Send<RequestJoinPlayer>(new() { deviceID = _deviceID });
    }
}
