using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputControllerDefault2 : BaseController
{
    // 在这里定义键盘码和逻辑的映射，后期可以魔改来支持调整键位
    const KeyCode left = KeyCode.LeftArrow;
    const KeyCode right = KeyCode.RightArrow;
    const KeyCode up = KeyCode.UpArrow;
    const KeyCode down = KeyCode.DownArrow;
    const KeyCode fire = KeyCode.Period;
    const KeyCode bomb = KeyCode.KeypadPeriod;
    // 这里往下应该使用如上定义的Keycode，不应该出现magic number

    public static bool JoinKeyDownTest() { return Input.GetKeyDown(fire); }
    public static bool JoinKeyUpTest() { return Input.GetKeyUp(fire); }

    public override void OnLogicFrameUpdate()
    {
        BindKey(fire, KeyType.FIRE);
        BindKey(bomb, KeyType.BOMB);
        BindKey(left, KeyType.LEFT);
        BindKey(right, KeyType.RIGHT);
    }

    public override bool OnExitDown() { return Input.GetKeyDown(bomb); }
    public override bool OnExitUp() { return Input.GetKeyUp(bomb); }

    public override bool OnUp() { return Input.GetKeyDown(up); } // A
    public override bool OnDown() { return Input.GetKeyDown(down); }
    public override float Horizon() { return (GetBitStatus(KeyType.LEFT) ? -1 : 0) + (GetBitStatus(KeyType.RIGHT) ? 1 : 0); } // 左右键长押tricky写法，可以优化掉
}
