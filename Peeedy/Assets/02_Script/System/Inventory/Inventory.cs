using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Cinemachine;
using UnityEditor.SearchService;
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

    private void Start()
    {
        DrawPage();
    }

    #region 툴 장착 및 해제
    private void ToolEquip()
    {
        if (_inventory.Count == 0 || _isActive == false) return;
        Tool currentTool = _inventory[_currentIndex];
        currentTool.gameObject.SetActive(true);
        player.ToolHandlerModule.HandleEquipTool(currentTool);

        _inventory.RemoveAt(_currentIndex);
        _currentIndex = _currentIndex == 0 ? 0 : _currentIndex - 1;
        DrawPage();
        Debug.Log($"Equipped {currentTool.name} from inventory.");
    }

    private void AddTool(ToolSaveToInventoryEvent @event)
    {
        _inventory.Add(@event.Tool);
        @event.Tool.gameObject.SetActive(false);
        player.ToolHandlerModule.UnEquipTool();
        DrawPage();

        Debug.Log($"Added {@event.Tool.name} to inventory.");
    }
    #endregion

    #region 인벤토리 조작 로직
    private void ToggleInventory()
    {
        _isActive = !_isActive;
        //float _sceneSpeed = _isActive ? 0f : 1f;
        ////Time.timeScale = _sceneSpeed;

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
        if (_inventory.Count == 0 || _isActive == false) return;

        _currentIndex = (_currentIndex + 1) % _inventory.Count;
        ToolSO currentToolSO = _inventory[_currentIndex].toolSO;

        DrawPage();
    }


    private void DrawPage()
    {
        if (_inventory.Count == 0)        {
            uiChannel.RaiseEvent(new InventoryNullDrawEvent());
            return;
        }

        uiChannel.RaiseEvent(UIEvents.InventoryDrawEvent.Init(_inventory[_currentIndex].toolSO, (_currentIndex + 1), _inventory.Count));
    }
    #endregion
}