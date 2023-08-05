using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 把状态暂存，等物理帧过来拉
// 下一帧不清上一帧的状态，只有按键发生变化才清，Trigger直接用扫描时_holdingTimeArr的帧号==当前帧号 来实现
// 我们需要保证全局的GlobalTick早于任何已有角色逻辑的Tick，在本地多人的Case中，每次开局用一个GO挂GlobalTick脚本，并且去Inspector中设置它的ExecutionOrder为优先于其它物件的逻辑即可
// 在联网的Case下（目前采用lock frame），我们用服务器下发的Tick逻辑来驱动其它本地世界的物件，不能使用FixedUpdate
public class StoreOnlyController
{
    GlobalVar gv; // 全局变量
    public StoreOnlyController(GlobalVar _gv) { gv = _gv; }

    private float _horizon = 0f;
    private uint[] _holdingTimeArr = new uint[System.Enum.GetNames(typeof(KeyType)).Length];

    public static uint GetBit(KeyType t) { return (uint)(1 << (int)t); }

    // === 内部使用函数，实际上用了_holdingTimeArr之后_keybitset就没用了
    void SetKeyDown(KeyType t) {  _holdingTimeArr[(uint)t] = gv.frameID; }
    void SetKeyUp(KeyType t) {  _holdingTimeArr[(uint)t] = 0; }

    bool OnTrigger(KeyType t) { return _holdingTimeArr[(uint)t] == gv.frameID - 1; } // 触发键逻辑，判断是不是上一帧开始产生的
    bool OnHolding(KeyType t) { return _holdingTimeArr[(uint)t] != 0; } // 0号帧是特殊的用于判断按键没有按下的帧，我们实际的Tick中令frameID从1开始
    
    // === 需要接入InputSystem回调的函数
    public void SetFire() { SetKeyDown(KeyType.FIRE); }
    public void UnsetFire() { SetKeyUp(KeyType.FIRE); }
    public void SetBomb() { SetKeyDown(KeyType.BOMB); }
    public void UnsetBomb() { SetKeyUp(KeyType.BOMB); }
    public void SetUp() { SetKeyDown(KeyType.UP); }
    public void UnSetUp() { SetKeyUp(KeyType.UP); }
    public void SetDown() { SetKeyDown(KeyType.DOWN); }
    public void UnSetDown() { SetKeyUp(KeyType.DOWN); }
    // 浮点（危
    public void SetHorizon(float h) { _horizon = h; }


    // 游戏内业务函数，实际游戏逻辑应从此处拉状态
    public float Horizon() { return _horizon; }
    public bool OnUp(){ return OnTrigger(KeyType.UP); } // 触发型按键，走OnTrigger
    public bool OnDown() { return OnTrigger(KeyType.DOWN); } 
    public bool OnFire() { return OnHolding(KeyType.FIRE); } // 长押型按键，走OnHolding
    public bool OnBomb() { return OnHolding(KeyType.BOMB); }

    // 按键时长，注意此处没有判断未按下的情况，此处返回的是按下的帧号时长，如果要算秒，应该乘算一个Time.fixedDeltaTime，但我推荐一切时间逻辑都直接拿帧来算
    public uint OnUpTime() { return gv.frameID - 1 - _holdingTimeArr[(uint)KeyType.UP]; }
    public uint OnDownTime() { return gv.frameID - 1 - _holdingTimeArr[(uint)KeyType.DOWN]; }
    public uint OnFireTime() { return gv.frameID - 1 - _holdingTimeArr[(uint)KeyType.FIRE]; }
    public uint OnBombTime() { return gv.frameID - 1 - _holdingTimeArr[(uint)KeyType.BOMB]; }

}
