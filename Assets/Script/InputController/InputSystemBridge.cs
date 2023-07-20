using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemBridge : MonoBehaviour
{
    private PlayerInput _playerInput;

    private PlayerPhysicalController _physicalCtrl;

    // Start is called before the first frame update
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        //var PlayerControllers = FindObjectsOfType<PlayerPhysicalController>();
        _physicalCtrl = GetComponent<PlayerPhysicalController>();
        //_physicalCtrl = PlayerControllers.FirstOrDefault(m => m.GetPlayerID() == _playerInput.playerIndex);
        Debug.Log("GOT CTRL " + _physicalCtrl);
    }

    public void Horizontal(InputAction.CallbackContext context)
    {
        //if (_physicalCtrl != null)
        //{
        var v = context.ReadValue<float>();
        Debug.Log("Horizontal" + v);
        _physicalCtrl.ctrl.SetHorizon(v);
        //}
    }
    // ����
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            //Debug.Log("Jump" + context);
            _physicalCtrl.ctrl.SetUp(); // �������������Բ��лص�}
        }
    }

    public void Down(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            //Debug.Log("Down" + context);
            _physicalCtrl.ctrl.SetDown(); // �������������Բ��лص�
        }
    }

    // ��ѹ
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
