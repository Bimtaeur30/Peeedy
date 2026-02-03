using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/Core/PlayerInputSO")]
public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
{
    public event Action JumpEvent;

    public Vector3 InputDirection { get; private set; }

    private Controls _controls;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 vec = context.ReadValue<Vector2>();
        InputDirection = new Vector3(vec.x, 0, vec.y);
    }
}
