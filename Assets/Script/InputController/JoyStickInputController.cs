using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickInputController : BaseController
{
    public JoyStickInputController(int id, string name)
    {
        SetJoystickInfo(id, name);
    }
    public float verticalAxisDeadZone = .6f;

    const int fire = 2; // 键号
    const int bomb = 1;
    const int jump = 0;

    const int joystick_offset = (int)KeyCode.Joystick1Button0; // 350 370 390 etc
    const int joystick_num_offset = (int)(KeyCode.Joystick2Button0 - KeyCode.Joystick1Button0);

    static KeyCode KC(int id, int k) => (KeyCode)(k + joystick_offset + (id-1) * joystick_num_offset);

    // 开火键测试，长押开火键加入
    public static bool JoinKeyDownTest(int id) { return Input.GetButtonDown($"joystick {id} button {fire}"); }
    public static bool JoinKeyUpTest(int id) { return Input.GetButtonUp($"joystick {id} button {fire}"); }


    public override void OnLogicFrameUpdate()
    {
        //if (_init)
        //{
        if (KD(fire)) _keybitset |= GetBit(KeyType.FIRE);
        if (KU(fire)) _keybitset &= (int)~GetBit(KeyType.FIRE);
        if (KD(bomb)) _keybitset |= GetBit(KeyType.BOMB);
        if (KU(bomb)) _keybitset &= (int)~GetBit(KeyType.BOMB);
        //}
    }
    //bool _init = false;
    int _id;
    string _name;
    public void SetJoystickInfo(int id, string name) { _id = id; _name = name;/* _init = true;*/ }
    bool KD(int b) { return Input.GetKeyDown(KC(_id, b)); }
    bool KU(int b) { return Input.GetKeyUp(KC(_id, b)); }
    public override bool OnExitDown() { return KD(bomb); }
    public override bool OnExitUp() { return KU(bomb); }
    public override bool OnUp() { return KD(jump) || Input.GetAxisRaw("Vertical_" + _name) > verticalAxisDeadZone; } // 跳跃别支持长押
    public override bool OnDown() { return -Input.GetAxisRaw("Vertical_" + _name) > verticalAxisDeadZone; }
    public override float Horizon() { return Input.GetAxisRaw("Horizontal_" + _name); }
}
