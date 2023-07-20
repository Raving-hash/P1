using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 把状态暂存，等物理帧过来拉
public class StoreOnlyController : BaseController
{
    public float verticalAxisDeadZone = .6f;

    const int down = 0b10;
    const int up = 0b1;

    private int _triggerbitset = 0;
    private float _horizon = 0f;

    // 触发
    public void SetUp() { _triggerbitset |= up; }
    public void SetDown() { _triggerbitset |= down; }

    // 长压
    public void SetFire() { _keybitset |= GetBit(KeyType.FIRE); }
    public void UnsetFire() { _keybitset &= ~GetBit(KeyType.FIRE); }
    public void SetBomb() { _keybitset |= GetBit(KeyType.BOMB); }
    public void UnsetBomb() { _keybitset &= ~GetBit(KeyType.BOMB); }

    // 浮点（危
    public void SetHorizon(float h) { _horizon = h; }

    public override void OnLogicFrameUpdate() { }

    public override float Horizon() { return _horizon; }

    public override bool OnUp()
    {
        if ((_triggerbitset & up) > 0)
        {
            _triggerbitset &= ~up; // 触发器，只消费一次，下次就得重新触发
            return true;
        }
        return false;
    } // 跳跃别支持长押

    public override bool OnDown()
    {
        if ((_triggerbitset & down) > 0)
        {
            _triggerbitset &= ~down;
            return true;
        }
        return false;
    }
}
