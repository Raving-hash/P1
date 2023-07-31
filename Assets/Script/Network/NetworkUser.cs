//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Mirror;
//using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

////enum ClientEvent
////{
////    LOGIN,
////}

//public class NetworkUser : NetworkBehaviour
//{
//    // Server stuff
//    // 服务器每帧都发一个指令，客户端等指令到了再tick
//    //ServerSingleton ss;

//    //[SyncVar]
//    public List<FrameOperation> ServersideOperationsBuffer = new List<FrameOperation>();
//    //[SyncVar]
//    public List<FrameOperation> ServersideOperationsHistory = new List<FrameOperation>();
//    public SyncDictionary<uint, Hashtable> ServersideOperationsHistoryByUser = new SyncDictionary<uint, Hashtable>();
//    //public bool serverIsInit = false;
//    public uint frameID;
//    public bool readyToTick = false;

//    void ResetServer()
//    {
//        ServersideOperationsBuffer.Clear();
//        ServersideOperationsHistory.Clear();
//        ServersideOperationsHistoryByUser.Clear();
//        frameID = 1;
//        readyToTick = true;
//        Debug.Log("SERVER INIT");
//    }

//    [ServerCallback]
//    List<FrameOperation> GetHis()
//    {
//        return ServersideOperationsHistory;
//    }

//    [ServerCallback]
//    private void OnEnable()
//    {
//        InvokeRepeating(nameof(ServerTick), 0f, 0.1f);    
//    }

//    [ServerCallback]
//    private void OnDisable()
//    {
//        // Stop the server tick loop when the script is disabled
//        CancelInvoke(nameof(ServerTick));
//    }

//    [ServerCallback]
//    public void ServerTick()
//    {
//        Debug.LogWarning($"queue len:{ServersideOperationsBuffer.Count}, history len:{ServersideOperationsHistory.Count}");
//        foreach (var x in ServersideOperationsBuffer)
//            ServersideOperationsHistory.Add(x);
//        if (ServersideOperationsBuffer.Count == 0)
//            ServersideOperationsBuffer.Add(new FrameOperation(0, "None", KeyType.EMPTY_FRAME, frameID));
//        RpcServerTick(new(ServersideOperationsBuffer));
//        ServersideOperationsBuffer.Clear();
//        ++frameID;
//    }

//    //[Server]
//    //private void FixedUpdate()
//    //{
//    //    if (isServer && readyToTick)
//    //    {
//    //        ServerTick();
//    //    }
//    //}

//    /*
//    * 从服务器拉其它玩家的状态
//    * 要返回一个哈希表，包括已有玩家的所有操作序列
//    */
//    [Command]
//    public void CmdFetchAll()
//    {
//        var his = GetHis();
//        Debug.Log($"fetchall to netid:{connectionToClient.identity.netId}, queue len:{his.Count}");
//        RpcInitAllPlayerFromServer(connectionToClient, new (his));
//    }

//    /*
//     * 保证Hashtable里有东西
//     * 玩家加入时处理
//     */
//    [Command]
//    public void CmdJoinPlayer(string deviceID)
//    {
//        Debug.LogWarning($"CMD JOIN PLAYER{ServersideOperationsHistory.Count}");
//        uint netID = NetworkClient.connection.identity.netId;
//        FrameOperation fopr = new FrameOperation(netID, deviceID, KeyType.JOIN, frameID);

//        Hashtable user_ht;
//        if (ServersideOperationsHistoryByUser.ContainsKey(netID))
//            user_ht = (Hashtable)ServersideOperationsHistoryByUser[netID];
//        else
//            ServersideOperationsHistoryByUser[netID] = user_ht = new Hashtable();
//        List<FrameOperation> player_ht;
//        if (user_ht.ContainsKey(deviceID))
//            player_ht = (List<FrameOperation>)user_ht[deviceID];
//        else
//            user_ht[deviceID] = player_ht = new List<FrameOperation>();
//        player_ht.Add(fopr);
//        ServersideOperationsBuffer.Add(fopr);
//    }

//    [Command]
//    public void CmdBroadcastLogout()
//    {
//        /*
//         * 广播全体有一个User端下线
//         * 需要在所有在线用户的世界中注销这个User建立的所有角色
//         */
//        uint netID = NetworkClient.connection.identity.netId;

//        foreach (DictionaryEntry playerEntry in (Hashtable)ServersideOperationsHistoryByUser[netID])
//        {
//            string deviceID = (string)playerEntry.Key;
//            FrameOperation fopr = new FrameOperation(netID, deviceID, KeyType.EXIT, frameID);
//            ((List<FrameOperation>)playerEntry.Value).Add(fopr);
//            ServersideOperationsBuffer.Add(fopr);
//        }
//        //if(ServersideOperationsHistory.ContainsKey(netID))
//        //ServersideOperationsHistory.
//        //RpcLogoutPlayer(netID);
//    }

//    [Command]
//    public void CmdPushOperation(Operation opr)
//    {
//        uint netID = NetworkClient.connection.identity.netId;
//        FrameOperation fopr = new FrameOperation(opr, frameID);
//        ServersideOperationsBuffer.Add(fopr);
//        ((List<FrameOperation>)((Hashtable)ServersideOperationsHistoryByUser[netID])[opr.deviceID]).Add(fopr);
//    }

//    //void Awake()
//    //{
//    //    if(!isInit)
//    //    {
//    //        isInit = true;
//    //    }
//    //}

//    public override void OnStartServer()
//    {
//        base.OnStartServer();
//        if (isServer)
//        {
//            //ss = FindFirstObjectByType<ServerSingleton>();
//            ResetServer();
//        }
//    }


//    // Client stuff

//    LocalSingleton singleton;

//    [TargetRpc]
//    public void RpcInitAllPlayerFromServer(NetworkConnectionToClient target, List<FrameOperation> history)
//    {
//        if (isLocalPlayer)
//        {
//            Debug.Log("PULL FROM SERVER:" + target + ", history len:" + history.Count);
//            singleton.InitAllPlayer(history);
//        }
//    }

//    // 在服务端调用该方法，然后通过Rpc发送消息给所有客户端
//    [ClientRpc]
//    public void RpcServerTick(List<FrameOperation> buffer)
//    {
//        if (isLocalPlayer)
//        {
//            Debug.Log($"Client Recv buffer len: {buffer.Count}");
//            singleton.BatchTick(buffer);
//        }
//    }

//    //[ClientRpc]
//    //public void RpcLogoutPlayer(uint netId)
//    //{
//    //    if (NetworkClient.connection.identity.netId == netId)
//    //        return;
//    //    _repo.LogoutPlayer(netId);
//    //}

//    public override void OnStopClient()
//    {
//        if (isLocalPlayer)
//        {
//            base.OnStopClient();
//            CmdBroadcastLogout();
//        }
//    }


//    public override void OnStartClient()
//    {
//        if (isLocalPlayer)
//        {
//            singleton = FindFirstObjectByType<LocalSingleton>();
//            singleton.localUser = this;
//            Debug.Log("Singleton init!frameid:" + singleton.localFrameID + " localuserid:" + singleton.localUser.netId);
//            CmdFetchAll();
//        }
//    }
//}
