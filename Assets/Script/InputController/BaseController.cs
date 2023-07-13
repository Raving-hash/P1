using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController
{
    public BaseController() { }
    public virtual void OnLogicFrameUpdate() { }
    protected int _keybitset = 0;

    protected int GetBit(KeyType t) { return (int)(1 << (int)t); }
    protected bool GetBitStatus(KeyType t) { return (_keybitset & (int)(1 << (int)t)) > 0; }

    // 修改bitset，用于支持长押
    protected void BindKey(KeyCode c, KeyType t)
    {
        if (Input.GetKeyDown(c)) _keybitset |= GetBit(t);
        else if (Input.GetKeyUp(c)) _keybitset &= (int)~GetBit(t);
    }

    public virtual bool OnExitDown() { return false; }
    public virtual bool OnExitUp() { return false; }

    public bool OnFire() { return GetBitStatus(KeyType.FIRE); }
    public bool OnBomb() { return GetBitStatus(KeyType.BOMB); }
    public virtual bool OnUp() { return false; }
    public virtual bool OnDown() { return false; }
    public virtual float Horizon() { return 0f; } // [-1, 1]

    public static BaseController ControllerFactory(string controllerid, string[] joysticknames)
    {
        if (controllerid.StartsWith("j"))
        {
            int id = int.Parse(controllerid[1..]); // C#里类似py的slice写法
            return new JoyStickInputController(id, joysticknames[id - 1]); // 手柄，id从1开始，所以拿下标要-1
        }
        else if(controllerid.StartsWith("k"))
        {
            int id = int.Parse(controllerid[1..]);
            switch (id)
            {
                case 1:
                    return new KeyboardInputControllerDefault1();
                case 2:
                //default:
                    return new KeyboardInputControllerDefault2();
            }
        }
        throw new System.Exception($"Unexpected key {controllerid} detected at BaseController.cs");
    }
}
