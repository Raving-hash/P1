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
    List<FrameOperation> ServersideOperationsBuffer;
    List<FrameOperation> ServersideOperationsHistory;
    Hashtable ServersideOperationsHistoryByUser;

    uint frameID;

    [Server]
    void ResetServer()
    {
        ServersideOperationsBuffer.Clear();
        ServersideOperationsHistory.Clear();
        ServersideOperationsHistoryByUser.Clear();
        frameID = 1;
    }

    [Server]
    private void FixedUpdate()
    {
        if(isServer)
        {
            Debug.LogWarning("queue len:" + ServersideOperationsBuffer.Count);
            if (ServersideOperationsBuffer.Count == 0)
                ServersideOperationsBuffer.Add(new FrameOperation(0, "None", KeyType.EMPTY_FRAME, frameID));
            RpcServerTick(ServersideOperationsBuffer);
            foreach (var x in ServersideOperationsBuffer)
                ServersideOperationsHistory.Add(x);
            ServersideOperationsBuffer.Clear();
            ++frameID;
        }
    }

    /*
    * 从服务器拉其它玩家的状态
    * 要返回一个哈希表，包括已有玩家的所有操作序列
    */
    [Command]
    public void CmdFetchAll()
    {
        RpcInitAllPlayerFromServer(NetworkClient.connection.identity.connectionToClient, ServersideOperationsHistory);
    }

    /*
     * 保证Hashtable里有东西
     * 玩家加入时处理
     */
    [Command]
    public void CmdJoinPlayer(string deviceID)
    {
        
        uint netID = NetworkClient.connection.identity.netId;
        FrameOperation fopr = new FrameOperation(netID, deviceID, KeyType.JOIN, frameID);

        Hashtable user_ht;
        if (ServersideOperationsHistoryByUser.ContainsKey(netID))
            user_ht = (Hashtable)ServersideOperationsHistoryByUser[netID];
        else
            ServersideOperationsHistoryByUser[netID] = user_ht = new Hashtable();
        List<FrameOperation> player_ht;
        if (user_ht.ContainsKey(deviceID))
            player_ht = (List<FrameOperation>)user_ht[deviceID];
        else
            user_ht[deviceID] = player_ht = new List<FrameOperation>();
        player_ht.Add(fopr);
        ServersideOperationsBuffer.Add(fopr);
    }

    [Command]
    public void CmdBroadcastLogout()
    {
        /*
         * 广播全体有一个User端下线
         * 需要在所有在线用户的世界中注销这个User建立的所有角色
         */
        uint netID = NetworkClient.connection.identity.netId;

        foreach(DictionaryEntry playerEntry in (Hashtable)ServersideOperationsHistoryByUser[netID])
        {
            string deviceID = (string)playerEntry.Key;
            FrameOperation fopr = new FrameOperation(netID, deviceID, KeyType.EXIT, frameID);
            ((List<FrameOperation>)playerEntry.Value).Add(fopr);
            ServersideOperationsBuffer.Add(fopr);
        }
        //if(ServersideOperationsHistory.ContainsKey(netID))
            //ServersideOperationsHistory.
        //RpcLogoutPlayer(netID);
    }

    [Command]
    public void CmdPushOperation(Operation opr)
    {
        uint netID = NetworkClient.connection.identity.netId;

        FrameOperation fopr = new FrameOperation(opr, frameID);
        ServersideOperationsBuffer.Add(fopr);
        ((List<FrameOperation>)((Hashtable)ServersideOperationsHistoryByUser[netID])[opr.deviceID]).Add(fopr);
    }

    //[Command]
    //public void CmdSendMessageToServer(string message)
    //{
    //    // 在服务端收到消息后，广播该消息给所有客户端
    //    Debug.Log($"Server: [{message}] from {NetworkClient.connection.identity.netId}");
    //    RpcBroadcast(message);
    //}


    // Client stuff

    LocalSingleton singleton;

    [TargetRpc]
    public void RpcInitAllPlayerFromServer(NetworkConnectionToClient target, List<FrameOperation>history)
    {
        singleton.InitAllPlayer(history);
    }

    // 在服务端调用该方法，然后通过Rpc发送消息给所有客户端
    [ClientRpc]
    public void RpcServerTick(List<FrameOperation> buffer)
    {
        if(isLocalPlayer)
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
        base.OnStopClient();
        CmdBroadcastLogout();
    }


    NetworkIdentity id;


    public override void OnStartClient()
    {
        id = NetworkClient.connection.identity;
        if(isLocalPlayer)
        {
            singleton = FindFirstObjectByType<LocalSingleton>();
            singleton.localUser = this;
            Debug.Log("Singleton init!" + singleton.localFrameID + " " + singleton.localUser);
        }
        
        Debug.Log("Net ID:" + id.netId);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        ServersideOperationsHistoryByUser = new Hashtable();
        ServersideOperationsBuffer = new List<FrameOperation>();
        ServersideOperationsHistory = new List<FrameOperation>();
        ResetServer();
    }



}
