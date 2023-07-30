using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemBridgeForNetwork : MonoBehaviour
{
    private PlayerInput _playerInput;
    //private LocalSingleton _singleton;

    private NetworkUser _user;

    // Start is called before the first frame update
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        Debug.Log("Input Scheme:"+_playerInput.currentControlScheme);
        var _singleton = FindFirstObjectByType<LocalSingleton>();
        _user = _singleton.localUser;
    }

    public void Horizontal(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        Debug.Log("Horizontal" + v);
        if (v != 0)
        {
            _user.CmdPushOperation(new Operation(_user.netId, _playerInput.currentControlScheme, v));
        }
    }
    // ´¥·¢
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _user.CmdPushOperation(new Operation(_user.netId, _playerInput.currentControlScheme, KeyType.UP));
        }
    }

    public void Down(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _user.CmdPushOperation(new Operation(_user.netId, _playerInput.currentControlScheme, KeyType.DOWN));
        }
    }

    // ³¤Ñ¹
    public void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire holding");
        if (context.phase == InputActionPhase.Started)
            _user.CmdPushOperation(new Operation(_user.netId, _playerInput.currentControlScheme, KeyType.FIRE_KEYDOWN));
        else if (context.phase == InputActionPhase.Canceled)
            _user.CmdPushOperation(new Operation(_user.netId, _playerInput.currentControlScheme, KeyType.FIRE_KEYUP));
    }

    public void Bomb(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            _user.CmdPushOperation(new Operation(_user.netId, _playerInput.currentControlScheme, KeyType.BOMB_KEYDOWN));
        else if (context.phase == InputActionPhase.Canceled)
            _user.CmdPushOperation(new Operation(_user.netId, _playerInput.currentControlScheme, KeyType.BOMB_KEYUP));
    }


}
