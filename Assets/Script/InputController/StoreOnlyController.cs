using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ��״̬�ݴ棬������֡������
public class StoreOnlyController : BaseController
{
    public float verticalAxisDeadZone = .6f;

    const int down = 0b10;
    const int up = 0b1;

    private int _triggerbitset = 0;
    private float _horizon = 0f;

    // ����
    public void SetUp() { _triggerbitset |= up; }
    public void SetDown() { _triggerbitset |= down; }

    // ��ѹ
    public void SetFire() { _keybitset |= GetBit(KeyType.FIRE); }
    public void UnsetFire() { _keybitset &= ~GetBit(KeyType.FIRE); }
    public void SetBomb() { _keybitset |= GetBit(KeyType.BOMB); }
    public void UnsetBomb() { _keybitset &= ~GetBit(KeyType.BOMB); }

    // ���㣨Σ
    public void SetHorizon(float h) { _horizon = h; }

    public override void OnLogicFrameUpdate() { }

    public override float Horizon() { return _horizon; }

    public override bool OnUp()
    {
        if ((_triggerbitset & up) > 0)
        {
            _triggerbitset &= ~up; // ��������ֻ����һ�Σ��´ξ͵����´���
            return true;
        }
        return false;
    } // ��Ծ��֧�ֳ�Ѻ

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
