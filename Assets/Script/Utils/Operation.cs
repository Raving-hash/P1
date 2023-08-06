using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operation
{
    public float horizontal; // [-1, 1] 轮询
    public uint keyset = 0; // 每个bit存一位状态，减少传输开销
    public uint netID;
    public string deviceID;

    public Operation() { }
    public Operation(uint _netID, string _deviceID, float _horizontal) { netID = _netID; deviceID = _deviceID; horizontal = _horizontal; }
    public Operation(uint _netID, string _deviceID) { netID = _netID; deviceID = _deviceID; }
    public Operation(uint _netID, string _deviceID, KeyType t) { netID = _netID; deviceID = _deviceID; keyset |= StoreOnlyController.GetBit(t); }
    public Operation(uint _netID, string _deviceID, uint _keyset) { netID = _netID; deviceID = _deviceID; keyset = _keyset; }
    public Operation(uint _netID, string _deviceID, float _horizontal, uint _keyset) { netID = _netID; deviceID = _deviceID; horizontal = _horizontal; keyset = _keyset; }
    public Operation(Operation father)
    {
        horizontal = father.horizontal;
        keyset = father.keyset;
        netID = father.netID;
        deviceID = father.deviceID;
    }
}