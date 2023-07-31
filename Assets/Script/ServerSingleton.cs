using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSingleton : MonoBehaviour
{
    public List<FrameOperation> ServersideOperationsBuffer;
    public List<FrameOperation> ServersideOperationsHistory;
    public Hashtable ServersideOperationsHistoryByUser;
    public bool serverIsInit = false;
    public uint frameID;
    public bool readyToTick = false;

    public void ResetServer()
    {
        if(!serverIsInit)
        {
            ServersideOperationsHistoryByUser = new Hashtable();
            ServersideOperationsBuffer = new List<FrameOperation>();
            ServersideOperationsHistory = new List<FrameOperation>();
            serverIsInit = true;
        }
        ServersideOperationsBuffer.Clear();
        ServersideOperationsHistory.Clear();
        ServersideOperationsHistoryByUser.Clear();
        frameID = 1;
        readyToTick = true;
        Debug.Log("SERVER INIT");
    }
   
}
