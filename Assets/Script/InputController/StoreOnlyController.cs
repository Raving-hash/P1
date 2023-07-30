using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 把状态暂存，等物理帧过来拉
public class StoreOnlyController : BaseController
{
    public float verticalAxisDeadZone = .6f;

    //const int down = 0b10;
    //const int up = 0b1;

    //private int _triggerbitset = 0;
    private float _horizon = 0f;

    // 触发
    public void SetUp() { keyset |= GetBit(KeyType.UP); }
    public void SetDown() { keyset |= GetBit(KeyType.DOWN); }

    //public void ResetTriggers()
    //{
    //    keyset &= ~(GetBit(KeyType.FIRE_KEYDOWN) | GetBit(KeyType.FIRE_KEYUP) | GetBit(KeyType.BOMB_KEYDOWN) | GetBit(KeyType.BOMB_KEYUP));
    //}

    // 长压
    public void SetFire() { keyset |= GetBit(KeyType.FIRE); }
    public void UnsetFire() { keyset &= ~GetBit(KeyType.FIRE); }
    public void SetBomb() { keyset |= GetBit(KeyType.BOMB); }
    public void UnsetBomb() { keyset &= ~GetBit(KeyType.BOMB); }

    // 浮点（危
    public void SetHorizon(float h) { _horizon = h; }

    public override void OnLogicFrameUpdate() { }

    public override float Horizon() { return _horizon; }

    bool OnTrigger(KeyType t)
    {
        if ((keyset & GetBit(t)) > 0)
        {
            keyset &= ~GetBit(t); // 触发器，只消费一次，下次就得重新触发
            return true;
        }
        return false;
    }

    public override bool OnUp() { return OnTrigger(KeyType.UP); } // 跳跃别支持长押

    public override bool OnDown() { return OnTrigger(KeyType.DOWN); }

    public bool OnFireKeyDown() { return OnTrigger(KeyType.FIRE_KEYDOWN); }
    public bool OnFireKeyUp() { return OnTrigger(KeyType.FIRE_KEYUP); }
    public bool OnBombKeyDown() { return OnTrigger(KeyType.BOMB_KEYDOWN); }
    public bool OnBombKeyUp() { return OnTrigger(KeyType.BOMB_KEYUP); }

}
