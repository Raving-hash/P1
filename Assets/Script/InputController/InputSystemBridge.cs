using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemBridge : MonoBehaviour
{
    private PlayerPhysicalController _physicalCtrl;

    // Start is called before the first frame update
    private void Awake()
    {
        //_playerInput = GetComponent<PlayerInput>();
        //var PlayerControllers = FindObjectsOfType<PlayerPhysicalController>();
        _physicalCtrl = GetComponent<PlayerPhysicalController>();
        //_physicalCtrl = PlayerControllers.FirstOrDefault(m => m.GetPlayerID() == _playerInput.playerIndex);
        Debug.Log("GOT CTRL " + _physicalCtrl);
    }

    public void Horizontal(InputAction.CallbackContext context)
    {
        var v = context.ReadValue<float>();
        Debug.Log("Horizontal" + v);
        _physicalCtrl.ctrl.SetHorizon(v);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            _physicalCtrl.ctrl.SetUp();
        else if (context.phase == InputActionPhase.Canceled)
            _physicalCtrl.ctrl.UnSetUp();
    }

    public void Down(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            _physicalCtrl.ctrl.SetDown();
        else if (context.phase == InputActionPhase.Canceled)
            _physicalCtrl.ctrl.UnSetDown();
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            _physicalCtrl.ctrl.SetFire();
        else if(context.phase == InputActionPhase.Canceled)
            _physicalCtrl.ctrl.UnsetFire();
    }

    public void Bomb(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            _physicalCtrl.ctrl.SetBomb();
        else if (context.phase == InputActionPhase.Canceled)
            _physicalCtrl.ctrl.UnsetBomb();
    }


}
