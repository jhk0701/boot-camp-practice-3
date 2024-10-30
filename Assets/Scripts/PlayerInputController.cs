using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action OnOpenSettingEvent;

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
            OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        else if(context.phase == InputActionPhase.Canceled)
            OnMoveEvent?.Invoke(Vector2.zero);
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
            OnLookEvent?.Invoke(context.ReadValue<Vector2>());
        else if(context.phase == InputActionPhase.Canceled)
            OnLookEvent?.Invoke(Vector2.zero);
    }

    public void OnOpenSetting(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            OnOpenSettingEvent?.Invoke();
    }
}
