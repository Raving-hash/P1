using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class NetworkUser : MonoBehaviour
{
    NetworkBroadcaster bcc;
    NetworkIdentity id;

    Hashtable operations;

    void Start()
    {
        id = NetworkClient.connection.identity;
        bcc = GetComponent<NetworkBroadcaster>();
        Debug.Log("Net ID:" + id.netId);
    }


}
