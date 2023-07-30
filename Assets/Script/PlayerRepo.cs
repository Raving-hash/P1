using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRepo
{
    // {
    //      netID(uint):{
    //          deviceID(str): PlayerDict
    //      }
    // }
    public Hashtable players = new Hashtable();
    // {
    //      netID(uint):{
    //          ID(str): [{}]
    //      }
    // }
    //public List<FrameOperation> localOperationBuffer;

    public PlayerDict GetPlayerDict(uint netID, string deviceID)
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

    public void TickAllPlayer(float deltaTime)
    {
        foreach (DictionaryEntry user_entry in players)
        {
            uint netID = (uint)user_entry.Key;
            Hashtable user_ht = (Hashtable)user_entry.Value;
            foreach (DictionaryEntry player_entry in user_ht)
            {
                string deviceID = (string)player_entry.Key;
                PlayerDict pd = (PlayerDict)player_entry.Value;
                pd.prefab.GetComponent<PlayerPhysicalController>().Tick(deltaTime);
            }
        }
    }

    public PlayerDict RegisterPlayer(GameObject prefab, uint netID, string deviceID)
    {
        var pd = GetPlayerDict(netID, deviceID);
        pd.prefab = GameObject.Instantiate(prefab);
        return pd;
    }

    public PlayerDict DestroyPlayer(uint netID, string deviceID)
    {
        var pd = GetPlayerDict(netID, deviceID);
        if (pd.prefab != null)
            GameObject.Destroy(pd.prefab);
        Hashtable user_ht = (Hashtable)players[netID];
        user_ht.Remove(deviceID);
        if (user_ht.Count == 0)
            players.Remove(netID);
        return pd;
    }

    //public void BatchTick()
    //{
    //    foreach(FrameOperation fopr in localOperationBuffer)
    //    {

    //        var pd = GetPlayerDict(fopr.netID, fopr.deviceID);

    //    }
    //}

    //public void LogoutPlayer(uint netId)
    //{

    //}
}
