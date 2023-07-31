using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSingleton : MonoBehaviour
{
    public NetworkUser localUser;
    public GameObject playerPrefab;
    public PlayerRepo localRepo = new PlayerRepo();
    public uint localFrameID = 0;

    public void BatchTick(List<FrameOperation> buf)
    {
        Debug.Log("batch tick:" + buf.Count);
        // 特判JOIN和EXIT两个操作
        foreach (var fopr in buf)
        {
            while (localFrameID < fopr.frameID)
            {
                localRepo.TickAllPlayer(Time.fixedDeltaTime);
                ++localFrameID;
            }
            Debug.Log("fopr:" + fopr.keyset);
            if ((fopr.keyset & BaseController.GetBit(KeyType.JOIN)) > 0)
                localRepo.RegisterPlayer(playerPrefab, fopr.netID, fopr.deviceID);
            else if ((fopr.keyset & BaseController.GetBit(KeyType.EXIT)) > 0)
                localRepo.DestroyPlayer(fopr.netID, fopr.deviceID);
            else if ((fopr.keyset & BaseController.GetBit(KeyType.EMPTY_FRAME)) > 0)
                continue;
            else
            {
                var pd = localRepo.GetPlayerDict(fopr.netID, fopr.deviceID);
                var physical_ctrl = pd.prefab.GetComponent<PlayerPhysicalController>();
                physical_ctrl.ctrl.keyset = fopr.keyset;
                physical_ctrl.ctrl.SetHorizon(fopr.horizontal);
            }

        }
    }

    public void InitAllPlayer(List<FrameOperation> history)
    {
        Debug.Log("init all player:" + history.Count);
        BatchTick(history);
        //foreach (DictionaryEntry user_entry in history)
        //{
        //    uint netID = (uint)user_entry.Key;
        //    Hashtable user_ht = (Hashtable)user_entry.Value;
        //    foreach (DictionaryEntry player_entry in user_ht)
        //    {
        //        string deviceID = (string)player_entry.Key;
        //        List<FrameOperation> li = (List<FrameOperation>)player_entry.Value;
        //        var pd = localRepo.RegisterPlayer(playerPrefab, netID, deviceID);
        //        pd.operationArray = li;
        //        localRepo.localOperationBuffer.AddRange(li);
        //    }
        //}
        //localRepo.localOperationBuffer.Sort((x, y) => { return (int)x.frameID - (int)y.frameID; });
    }
}
