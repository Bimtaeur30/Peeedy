using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Cinemachine;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private EventChannelSO uiChannel;
    [SerializeField] private EventChannelSO toolChannel;
    [SerializeField] private PlayerInputSO inputSO;
    private List<Tool> _inventory = new List<Tool>();
    private int _currentIndex = 0;
    private bool _isActive = false;

    private void OnEnable()
    {
        inputSO.OnInventoryToggleEvent += ToggleInventory;
        inputSO.OnInventoryPageUpEvent += PageUP;
        inputSO.OnToolEquipEvent += ToolEquip;

        toolChannel.AddListener<ToolSaveToInventoryEvent>(AddTool);
    }

    private void OnDisable()
    {
        inputSO.OnInventoryToggleEvent -= ToggleInventory;
        inputSO.OnInventoryPageUpEvent -= PageUP;
        inputSO.OnToolEquipEvent -= ToolEquip;

        toolChannel.RemoveListener<ToolSaveToInventoryEvent>(AddTool);
    }

    private void ToggleInventory()
    {
        _isActive = !_isActive;
        uiChannel.RaiseEvent(UIEvents.InventoryToggleEvent.Init(_isActive));

        if (_isActive)
        {
            cinemachineCamera.Priority = 2;
        }
        else
        {
            cinemachineCamera.Priority = 0;
        }
    }
    private void PageUP()
    {
        _currentIndex = (_currentIndex + 1) % _inventory.Count;
        ToolSO currentToolSO = _inventory[_currentIndex].toolSO;

        uiChannel.RaiseEvent(UIEvents.InventoryDrawEvent.Init(currentToolSO));
    }

    private void ToolEquip()
    {
        if (_isActive == false) return;
        Tool currentTool = _inventory[_currentIndex];

        player.ToolHandlerModule.HandleEquipTool(currentTool);
    }

    private void AddTool(ToolSaveToInventoryEvent @event)
    {
        _inventory.Add(@event.Tool);
        Debug.Log($"Added {@event.Tool.name} to inventory.");
    }
}
