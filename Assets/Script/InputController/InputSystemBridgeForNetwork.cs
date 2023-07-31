using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class InputSystemBridgeForNetwork : MonoBehaviour
{
    private LocalSingleton _singleton;
    public string deviceID;
    //private NetworkUser _user;


    // Start is called before the first frame update
    private void Awake()
    {
        //ctrl = GetComponentInParent<PlayerPhysicalController>();
        //Debug.Log("DEVICE:"+ctrl.deviceID);
        _singleton = FindFirstObjectByType<LocalSingleton>();
        //_user = _singleton.localUser;
    }

    public void Horizontal(InputAction.CallbackContext context)
    {
        if (deviceID == null) return;

        var v = context.ReadValue<float>();
        Debug.Log("Horizontal" + v);
        if (v != 0)
            _singleton.CmdPushOperation(new Operation(NetworkClient.connection.identity.netId, deviceID, v));
    }
    // 触发
    public void Jump(InputAction.CallbackContext context)
    {
        if (deviceID == null) return;
        if (context.phase == InputActionPhase.Started)
            _singleton.CmdPushOperation(new Operation(NetworkClient.connection.identity.netId, deviceID, KeyType.UP));
    }

    public void Down(InputAction.CallbackContext context)
    {
        if (deviceID == null) return;

        if (context.phase == InputActionPhase.Started)
            _singleton.CmdPushOperation(new Operation(NetworkClient.connection.identity.netId, deviceID, KeyType.DOWN));
    }

    // 长压
    public void Fire(InputAction.CallbackContext context)
    {
        if (deviceID == null) return;
        Debug.Log("Fire holding");
        if (context.phase == InputActionPhase.Started)
            _singleton.CmdPushOperation(new Operation(NetworkClient.connection.identity.netId, deviceID, KeyType.FIRE_KEYDOWN));
        else if (context.phase == InputActionPhase.Canceled)
            _singleton.CmdPushOperation(new Operation(NetworkClient.connection.identity.netId, deviceID, KeyType.FIRE_KEYUP));
    }

    public void Bomb(InputAction.CallbackContext context)
    {
        if (deviceID == null) return;
        if (context.phase == InputActionPhase.Started)
            _singleton.CmdPushOperation(new Operation(NetworkClient.connection.identity.netId, deviceID, KeyType.BOMB_KEYDOWN));
        else if (context.phase == InputActionPhase.Canceled)
            _singleton.CmdPushOperation(new Operation(NetworkClient.connection.identity.netId, deviceID, KeyType.BOMB_KEYUP));
    }


}
