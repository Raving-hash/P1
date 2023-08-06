using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;


public class JoiningGame_InputSystem : MonoBehaviour
{
    Dictionary<InputDevice, List<string>> joinedDevices = new Dictionary<InputDevice, List<string>>();
    public GameObject HeroPrefab;

    void Update()
    {// Check for J key to join Player 1 with Keyboard1 preset
        if (Keyboard.current.jKey.wasPressedThisFrame && !(joinedDevices.ContainsKey(Keyboard.current) && (joinedDevices[Keyboard.current]).Contains("KB0")))
        {
            JoinPlayer(joinedDevices.Count, "KB", Keyboard.current, "KB0");
        }

        // Check for . key to join Player 2 with Keyboard2 preset
        if (Keyboard.current.periodKey.wasPressedThisFrame && !(joinedDevices.ContainsKey(Keyboard.current) && (joinedDevices[Keyboard.current]).Contains("KB1")))
        {
            JoinPlayer(joinedDevices.Count, "KB2", Keyboard.current, "KB1");
        }

        // Check for A button on connected gamepads to join a Player with Joystick preset
        //Debug.Log(Gamepad.all.Count);
        //Debug.Log("Joystick:"+Joystick.all.Count);
        foreach (var gamepad in Gamepad.all)
        {
            Debug.Log("Gamepad:" + gamepad + ", A Down:" + gamepad.aButton.wasPressedThisFrame);
            if (gamepad.aButton.wasPressedThisFrame && !joinedDevices.ContainsKey(gamepad))
            {
                JoinPlayer(joinedDevices.Count, "JS", gamepad, gamepad.displayName);
            }
        }
    }

    private void JoinPlayer(int playerIndex, string controlScheme, InputDevice device, string deviceID)
    {
        // Create a new PlayerInput instance for the player
        //Debug.Log("New player:" + controlScheme + ", Device:" + device);
        PlayerInput playerInput = PlayerInput.Instantiate(
            prefab: HeroPrefab, // Replace with your player prefab if needed
            controlScheme: controlScheme,
            pairWithDevice: device
        );

        var singleton = FindFirstObjectByType<LocalSingleton>();
        //Debug.Log("playerInput:" + playerInput);
        //Debug.Log("playerInput gameObject:" + playerInput.gameObject);
        playerInput.gameObject.GetComponent<InputSystemBridgeForNetwork>().deviceID = deviceID;
        singleton.CmdJoinPlayer(deviceID);

        // Add the device to the joinedDevices list
        if (device != null)
        {
            if (!joinedDevices.ContainsKey(device))
                joinedDevices.Add(device, new List<string>());
            (joinedDevices[device]).Add(deviceID);
        }
    }

}
