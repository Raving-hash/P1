using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 加入游戏场景的主逻辑代码
public class JoiningGame : MonoBehaviour
{
    public float pressingThreshold = 1f; // How many seconds should players press to join
    //public int maxJoystickCount = 4; // How many joystick players supported
    Hashtable pressingRecord;

    public GameObject playerAvatarPrefab; // 这个后期改成复数用random选一个

    Hashtable localPlayers; // 放本机操控的玩家，之后考虑移出去

    void Start()
    {
        pressingRecord = new Hashtable();
        localPlayers = new Hashtable();
    }

    void Update()
    {
        HoldFireToJoinEvent();
        HoldBombToLeaveEvent();
    }
    void HoldFireToJoinEvent()
    {
        //string[] joystick_names = Input.GetJoystickNames();
        string[] joystick_names = { };
        for (int i = 1; i <= joystick_names.Length; ++i)
        {
            string k = $"j{i}";
            if (!pressingRecord.ContainsKey(k))
            {
                if (JoyStickInputController.JoinKeyDownTest(i))
                    pressingRecord.Add(k, Time.time);
            }
            else if (JoyStickInputController.JoinKeyUpTest(i))
                pressingRecord.Remove(k);
        }

        string k1 = "k1";
        if (!pressingRecord.ContainsKey(k1))
        {
            if (KeyboardInputControllerDefault1.JoinKeyDownTest())
                pressingRecord.Add(k1, Time.time);
        }
        else if (KeyboardInputControllerDefault1.JoinKeyUpTest())
            pressingRecord.Remove(k1);

        string k2 = "k2";
        if (!pressingRecord.ContainsKey(k2))
        {
            if (KeyboardInputControllerDefault2.JoinKeyDownTest())
                pressingRecord.Add(k2, Time.time);
        }
        else if (KeyboardInputControllerDefault2.JoinKeyUpTest())
            pressingRecord.Remove(k2);

        //DictionaryEntry[] entries = new DictionaryEntry[pressingRecord.Count];
        //pressingRecord.CopyTo(entries, 0);
        //Debug.Log(pressingRecord.Count);
        foreach (DictionaryEntry entry in pressingRecord)
        {
            string key = (string)entry.Key;
            if (localPlayers.ContainsKey(key)) continue;
            float delta = Time.time - (float)entry.Value;
            if (delta >= pressingThreshold)
            {
                GameObject obj = Instantiate(playerAvatarPrefab, transform, true);
                PlayerPhysicalController player_script = obj.GetComponent<PlayerPhysicalController>();
                player_script.ctrl = BaseController.ControllerFactory(key, joystick_names);
                localPlayers.Add(key, obj);
            }
        }

    }
    // 根据localPlayers来处理退出
    void HoldBombToLeaveEvent()
    {

    }

}
