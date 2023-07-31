using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRepo
{
    public Dictionary<uint, Dictionary<string, PlayerDict>> players = new();

    public PlayerDict GetPlayerDict(uint netID, string deviceID)
    {
        Dictionary<string, PlayerDict> player_netid;
        if (!players.ContainsKey(netID))
            players[netID] = player_netid = new Dictionary<string, PlayerDict>();
        else
            player_netid = players[netID];

        PlayerDict pd;
        if (!player_netid.ContainsKey(deviceID))
            player_netid[deviceID] = pd = new PlayerDict();
        else
            pd = player_netid[deviceID];
        return pd;
    }

    public void TickAllPlayer(float deltaTime)
    {
        foreach (var user_entry in players)
        {
            uint netID = user_entry.Key;
            Dictionary<string, PlayerDict> user_ht = user_entry.Value;
            foreach (var player_entry in user_ht)
            {
                string deviceID = player_entry.Key;
                PlayerDict pd = player_entry.Value;
                var ctrl = pd.prefab.GetComponent<PlayerPhysicalController>();
                ctrl.Tick(deltaTime);
                ctrl.ctrl.RefreshTriggers();
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
        Dictionary<string, PlayerDict> user_ht = players[netID];
        user_ht.Remove(deviceID);
        if (user_ht.Count == 0)
            players.Remove(netID);
        return pd;
    }
}
