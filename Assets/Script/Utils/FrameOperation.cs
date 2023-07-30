using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameOperation: Operation
{
    public uint frameID;
    public FrameOperation() { }
    public FrameOperation(Operation father): base(father) {  }
    public FrameOperation(Operation father, uint _frameID) : base(father) { frameID = _frameID; }
    public FrameOperation(uint _netID, string _deviceID, KeyType _t, uint _frameID): base(_netID, _deviceID, _t) { frameID = _frameID; }
}