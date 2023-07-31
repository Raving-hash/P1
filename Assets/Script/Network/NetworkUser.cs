using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//enum ClientEvent
//{
//    LOGIN,
//}

public class NetworkUser : NetworkBehaviour
{
    // Server stuff
    // 服务器每帧都发一个指令，客户端等指令到了再tick
    //ServerSingleton ss;

    

    [Server]
    private void FixedUpdate()
    {
        if (isServer && ss.readyToTick)
        {
            Debug.LogWarning($"queue len:{ss.ServersideOperationsBuffer.Count}, history len:{ss.ServersideOperationsHistory.Count}");
            foreach (var x in ss.ServersideOperationsBuffer)
                ss.ServersideOperationsHistory.Add(x);
            if (ss.ServersideOperationsBuffer.Count == 0)
                ss.ServersideOperationsBuffer.Add(new FrameOperation(0, "None", KeyType.EMPTY_FRAME, ss.frameID));
            RpcServerTick(ss.ServersideOperationsBuffer);
            ss.ServersideOperationsBuffer.Clear();
            ++ss.frameID;
        }
    }

    /*
    * 从服务器拉其它玩家的状态
    * 要返回一个哈希表，包括已有玩家的所有操作序列
    */
    [Command]
    public void CmdFetchAll()
    {
        Debug.Log($"fetchall to netid:{NetworkClient.connection.identity.netId}, queue len:{ss.ServersideOperationsHistory.Count}");
        RpcInitAllPlayerFromServer(connectionToClient, ss.ServersideOperationsHistory);
    }

    /*
     * 保证Hashtable里有东西
     * 玩家加入时处理
     */
    [Command]
    public void CmdJoinPlayer(string deviceID)
    {
        uint netID = NetworkClient.connection.identity.netId;
        FrameOperation fopr = new FrameOperation(netID, deviceID, KeyType.JOIN, ss.frameID);

        Hashtable user_ht;
        if (ss.ServersideOperationsHistoryByUser.ContainsKey(netID))
            user_ht = (Hashtable)ss.ServersideOperationsHistoryByUser[netID];
        else
            ss.ServersideOperationsHistoryByUser[netID] = user_ht = new Hashtable();
        List<FrameOperation> player_ht;
        if (user_ht.ContainsKey(deviceID))
            player_ht = (List<FrameOperation>)user_ht[deviceID];
        else
            user_ht[deviceID] = player_ht = new List<FrameOperation>();
        player_ht.Add(fopr);
        ss.ServersideOperationsBuffer.Add(fopr);
    }

    [Command]
    public void CmdBroadcastLogout()
    {
        /*
         * 广播全体有一个User端下线
         * 需要在所有在线用户的世界中注销这个User建立的所有角色
         */
        uint netID = NetworkClient.connection.identity.netId;

        foreach (DictionaryEntry playerEntry in (Hashtable)ss.ServersideOperationsHistoryByUser[netID])
        {
            string deviceID = (string)playerEntry.Key;
            FrameOperation fopr = new FrameOperation(netID, deviceID, KeyType.EXIT, ss.frameID);
            ((List<FrameOperation>)playerEntry.Value).Add(fopr);
            ss.ServersideOperationsBuffer.Add(fopr);
        }
        //if(ServersideOperationsHistory.ContainsKey(netID))
        //ServersideOperationsHistory.
        //RpcLogoutPlayer(netID);
    }

    [Command]
    public void CmdPushOperation(Operation opr)
    {
        uint netID = NetworkClient.connection.identity.netId;
        FrameOperation fopr = new FrameOperation(opr, ss.frameID);
        ss.ServersideOperationsBuffer.Add(fopr);
        ((List<FrameOperation>)((Hashtable)ss.ServersideOperationsHistoryByUser[netID])[opr.deviceID]).Add(fopr);
    }

    //void Awake()
    //{
    //    if(!isInit)
    //    {
    //        isInit = true;
    //    }
    //}

    public override void OnStartServer()
    {
        base.OnStartServer();
        if (isServer)
        {
            ss = FindFirstObjectByType<ServerSingleton>();
            ss.ResetServer();
        }
    }


    // Client stuff

    LocalSingleton singleton;

    [TargetRpc]
    public void RpcInitAllPlayerFromServer(NetworkConnectionToClient target, List<FrameOperation> history)
    {
        if (isLocalPlayer)
        {
            Debug.Log("PULL FROM SERVER:" + target + ", history len:" + history.Count);
            singleton.InitAllPlayer(history);
        }
    }

    // 在服务端调用该方法，然后通过Rpc发送消息给所有客户端
    [ClientRpc]
    public void RpcServerTick(List<FrameOperation> buffer)
    {
        if (isLocalPlayer)
        {
            Debug.Log($"Client Recv buffer len: {buffer.Count}");
            singleton.BatchTick(buffer);
        }
    }

    //[ClientRpc]
    //public void RpcLogoutPlayer(uint netId)
    //{
    //    if (NetworkClient.connection.identity.netId == netId)
    //        return;
    //    _repo.LogoutPlayer(netId);
    //}

    public override void OnStopClient()
    {
        if (isLocalPlayer)
        {
            base.OnStopClient();
            CmdBroadcastLogout();
        }
    }


    public override void OnStartClient()
    {
        if (isLocalPlayer)
        {
            singleton = FindFirstObjectByType<LocalSingleton>();
            singleton.localUser = this;
            Debug.Log("Singleton init!frameid:" + singleton.localFrameID + " localuserid:" + singleton.localUser.netId);
            CmdFetchAll();
        }
    }
}
