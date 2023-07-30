using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operation
{
    public float horizontal; // [-1, 1] 轮询
    public int keyset = 0; // 每个bit存一位状态，减少传输开销
    public uint netID;
    public string deviceID;

    public Operation() { }
    public Operation(uint _netID, string _deviceID, float _horizontal) { netID = _netID; deviceID = _deviceID; horizontal = _horizontal; }
    public Operation(uint _netID, string _deviceID) { netID = _netID; deviceID = _deviceID; }
    public Operation(uint _netID, string _deviceID, KeyType t) { netID = _netID; deviceID = _deviceID; keyset |= BaseController.GetBit(t); }
    public Operation(uint _netID, string _deviceID, int _keyset) { netID = _netID; deviceID = _deviceID; keyset = _keyset; }
    public Operation(uint _netID, string _deviceID, float _horizontal, int _keyset) { netID = _netID; deviceID = _deviceID; horizontal = _horizontal; keyset = _keyset; }
    public Operation(Operation father)
    {
        horizontal = father.horizontal;
        keyset = father.keyset;
        netID = father.netID;
        deviceID = father.deviceID;
    }
}

    //public static Operation ScanOperation(StoreOnlyController ctrl)
    //{
    //    Operation opr = new Operation();
    //    if (ctrl.OnBombKeyDown()) opr.keyset |= BaseController.GetBit(KeyType.BOMB_KEYDOWN); // 长按按键用Up和Down两个状态封好，避免逐帧传输
    //    if (ctrl.OnBombKeyUp()) opr.keyset |= BaseController.GetBit(KeyType.BOMB_KEYUP);
    //    if (ctrl.OnFireKeyDown()) opr.keyset |= BaseController.GetBit(KeyType.FIRE_KEYDOWN);
    //    if (ctrl.OnFireKeyUp()) opr.keyset |= BaseController.GetBit(KeyType.FIRE_KEYUP);
    //    if (ctrl.OnUp()) opr.keyset |= BaseController.GetBit(KeyType.UP);
    //    if (ctrl.OnDown()) opr.keyset |= BaseController.GetBit(KeyType.DOWN);

    //    opr.horizontal = ctrl.Horizon();

    //    return opr;
    //    //foreach(KeyType t in Enum.GetValues(typeof(KeyType)))
    //}
