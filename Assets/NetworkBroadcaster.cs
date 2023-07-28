using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkBroadcaster : NetworkBehaviour
{
    // 在客户端调用该方法
    //[Command(requiresAuthority = false)]
    [Command]
    public void CmdSendMessageToServer(string message)
    {
        // 在服务端收到消息后，广播该消息给所有客户端
        Debug.Log($"Server: {message}");

        RpcSendMessageToAllClients(message);
    }

    // 在服务端调用该方法，然后通过Rpc发送消息给所有客户端
    [ClientRpc]
    public void RpcSendMessageToAllClients(string message)
    {
        // 在这里处理接收到的消息，并广播给所有客户端
        Debug.Log($"Client: {message}");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T Down");
            CmdSendMessageToServer("hehe");
        }
    }
}
