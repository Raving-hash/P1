using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRepo
{
    // {
    //      netID(uint):{
    //          ID(str): PlayerDict
    //      }
    // }
    public Hashtable players;
    // {
    //      netID(uint):{
    //          ID(str): [{}]
    //      }
    // }
    public List<FrameOperation> localOperationBuffer;

    PlayerDict GetPlayerDict(uint netID, string deviceID)
    {
        Hashtable player_netid;
        if (!players.ContainsKey(netID))
            players[netID] = player_netid = new Hashtable();
        else
            player_netid = (Hashtable)players[netID];

        PlayerDict pd;
        if (!player_netid.ContainsKey(deviceID))
            player_netid[deviceID] = pd = new PlayerDict();
        else
            pd = (PlayerDict)player_netid[deviceID];
        return pd;
    }

    public PlayerDict RegisterPlayer(GameObject prefab, uint netID, string deviceID)
    {
        var pd = GetPlayerDict(netID, deviceID);
        pd.prefab = GameObject.Instantiate(prefab);
        return pd;
    }

    public void BatchTick()
    {
        foreach(FrameOperation fopr in localOperationBuffer)
        {

            var pd = GetPlayerDict(fopr.netID, fopr.deviceID);

        }
    }

    //public void LogoutPlayer(uint netId)
    //{

    //}
}
