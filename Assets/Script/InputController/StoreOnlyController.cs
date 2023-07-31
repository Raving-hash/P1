using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 把状态暂存，等物理帧过来拉
public class StoreOnlyController : BaseController
{
    private float _horizon = 0f;

    // 触发
    public void SetUp() { keyset |= GetBit(KeyType.UP); }
    public void SetDown() { keyset |= GetBit(KeyType.DOWN); }

    public void RefreshTriggers()
    {
        if (OnFireKeyDown()) keyset |= GetBit(KeyType.FIRE);
        if (OnFireKeyUp()) keyset &= ~GetBit(KeyType.FIRE); 
        if (OnBombKeyDown()) keyset |= GetBit(KeyType.BOMB);
        if (OnBombKeyUp()) keyset &= ~GetBit(KeyType.BOMB);
        keyset &= ~(GetBit(KeyType.FIRE_KEYDOWN)
            | GetBit(KeyType.FIRE_KEYUP)
            | GetBit(KeyType.BOMB_KEYDOWN)
            | GetBit(KeyType.BOMB_KEYUP)
            | GetBit(KeyType.UP)
            | GetBit(KeyType.DOWN));
        //_horizon = 0f;
    }

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

    bool OnFireKeyDown() { return OnTrigger(KeyType.FIRE_KEYDOWN); }
    bool OnFireKeyUp() { return OnTrigger(KeyType.FIRE_KEYUP); }
    bool OnBombKeyDown() { return OnTrigger(KeyType.BOMB_KEYDOWN); }
    bool OnBombKeyUp() { return OnTrigger(KeyType.BOMB_KEYUP); }

}
