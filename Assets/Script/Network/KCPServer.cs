using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public struct RespServerTick : NetworkMessage
{
    public List<FrameOperation> buffer;
}

public struct RequestFetchAll : NetworkMessage { }
public struct RespFetchAll : NetworkMessage { public List<FrameOperation> history; }

public struct RequestJoinPlayer : NetworkMessage { public string deviceID; }
public struct RequestBroadcastLogout : NetworkMessage { }
public struct RequestPushOperation : NetworkMessage { public Operation opr; }

public class KCPServer : NetworkManager
{
    List<FrameOperation> ServerOPBuffer;
    List<FrameOperation> ServerOPHistory;
    Dictionary<uint, List<string>> ServerUserDevices;
    uint frameID;

    void ResetServer()
    {
        ServerOPBuffer.Clear();
        ServerOPHistory.Clear();
        ServerUserDevices.Clear();
        frameID = 1;
        Debug.Log("SERVER INIT");
    }

    private void ServerTick()
    {
        //Debug.LogWarning($"queue len:{ServerOPBuffer.Count}, history len:{ServerOPHistory.Count}");
        foreach (var x in ServerOPBuffer)
            ServerOPHistory.Add(x);
        if (ServerOPBuffer.Count == 0)
            ServerOPBuffer.Add(new FrameOperation(0, "None", KeyType.EMPTY_FRAME, frameID));
        //RpcServerTick(new(ServerOPBuffer));
        NetworkServer.SendToAll<RespServerTick>(new() { buffer = ServerOPBuffer });
        ServerOPBuffer.Clear();
        ++frameID;
    }

    // 自定义消息类型，用于客户端请求数据

    public void CmdFetchAll(NetworkConnection conn, RequestFetchAll req)
    {
        conn.Send<RespFetchAll>(new RespFetchAll() { history = ServerOPHistory });
    }

    public void CmdJoinPlayer(NetworkConnection conn, RequestJoinPlayer req)
    {
        uint netID = conn.identity.netId;
        string deviceID = req.deviceID;
        FrameOperation fopr = new FrameOperation(netID, deviceID, KeyType.JOIN, frameID);
        List<string> playerDevices;
        if (ServerUserDevices.ContainsKey(netID))
            playerDevices = ServerUserDevices[netID];
        else
            ServerUserDevices[netID] = playerDevices = new List<string>();
        playerDevices.Add(deviceID);
        ServerOPBuffer.Add(fopr);
    }

    public void CmdBroadcastLogout(NetworkConnection conn, RequestBroadcastLogout req)
    {
        uint netID = conn.identity.netId;
        foreach (var deviceID in ServerUserDevices[netID])
        {
            FrameOperation fopr = new FrameOperation(netID, deviceID, KeyType.EXIT, frameID);
            ServerOPBuffer.Add(fopr);
        }
        ServerUserDevices.Remove(netID);
    }

    public void CmdPushOperation(NetworkConnection conn, RequestPushOperation req)
    {
        uint netID = conn.identity.netId;
        FrameOperation fopr = new FrameOperation(req.opr, frameID);
        ServerOPBuffer.Add(fopr); // 可能会有数据竞争
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<RequestFetchAll>(CmdFetchAll);
        NetworkServer.RegisterHandler<RequestJoinPlayer>(CmdJoinPlayer);
        NetworkServer.RegisterHandler<RequestBroadcastLogout>(CmdBroadcastLogout);
        NetworkServer.RegisterHandler<RequestPushOperation>(CmdPushOperation);
        ServerOPBuffer = new List<FrameOperation>();
        ServerOPHistory = new List<FrameOperation>();
        ServerUserDevices = new Dictionary<uint, List<string>>();
        ResetServer();
        InvokeRepeating(nameof(ServerTick), 0f, 0.03f); // 常量记得外提
        //Debug.LogWarning("CUSTOM SERVER STARTED");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        CancelInvoke(nameof(ServerTick));
    }

    // Client
    LocalSingleton lst;
    public GameObject localListener;

    public void RpcFetchAll(RespFetchAll resp)
    {
        lst.BatchTick(resp.history);
    }

    public void RpcServerTick(RespServerTick resp)
    {
        lst.BatchTick(resp.buffer);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        lst = FindFirstObjectByType<LocalSingleton>();
        GameObject.Instantiate(localListener); // 监听键盘加入玩家
        NetworkClient.RegisterHandler<RespFetchAll>(RpcFetchAll);
        NetworkClient.RegisterHandler<RespServerTick>(RpcServerTick);
        NetworkClient.Send<RequestFetchAll>(new RequestFetchAll());
    }

}
