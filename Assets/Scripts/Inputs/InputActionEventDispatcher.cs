using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionEventDispatcher : MonoBehaviour
{
    protected PlayerInput inputs;
    public PlayerInput Inputs { get {  return inputs; } }
    private void OnEnable()
    {
        inputs = GetComponent<PlayerInput>();
        inputs.onActionTriggered += Inputs_onActionTriggered;
    }

    private void Inputs_onActionTriggered(InputAction.CallbackContext obj)
    {
        if (!inputs.isActiveAndEnabled) return;
        SendMessage($"On{obj.action.name}", obj, SendMessageOptions.RequireReceiver);
    }
}
