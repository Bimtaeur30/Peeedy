using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/Core/PlayerInputSO")]
public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
{
    public event Action LeftMouseClickEvent;

    public event Action OnToolEquipEvent;
    public event Action OnToolUnEquipEvent;
    public event Action OnBuildingEnterEvent;
    public event Action OnInventoryToggleEvent;
    public event Action OnInventoryPageUpEvent;
    public event Action OnToolSaveInventoryEvent;

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

    public void OnMouseLeftClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        LeftMouseClickEvent?.Invoke();
    }

    public void OnToolEquip(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnToolEquipEvent?.Invoke();
        }
    }

    public void OnToolUnEquip(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnToolUnEquipEvent?.Invoke();
        }
    }

    public void OnBuildingEnter(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnBuildingEnterEvent?.Invoke();
        }
    }

    public void OnInventoryToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnInventoryToggleEvent?.Invoke();
        }
    }

    public void OnInventoryPageUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnInventoryPageUpEvent?.Invoke();
        }
    }

    public void OnToolSaveInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnToolSaveInventoryEvent?.Invoke();
        }
    }
}
