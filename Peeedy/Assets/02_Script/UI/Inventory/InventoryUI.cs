using DG.Tweening;
using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private EventChannelSO uiChannel;
    [SerializeField] private CanvasGroup inventoryCanvasGroup;
    [SerializeField] private TextMeshProUGUI itemNameTxt;
    [SerializeField] private TextMeshProUGUI pageTxt;

    private void OnEnable()
    {
        uiChannel.AddListener<InventoryToggleEvent>(OnInventoryToggle);
        uiChannel.AddListener<InventoryDrawEvent>(OnInventoryDraw);
        uiChannel.AddListener<InventoryNullDrawEvent>(OnInventoryNullDraw);
    }

    private void OnInventoryNullDraw(InventoryNullDrawEvent @event)
    {
        pageTxt.text = "인벤토리(0/0)";
        itemNameTxt.text = "인벤토리가 비어있음";
    }

    private void OnInventoryDraw(InventoryDrawEvent @event)
    {
        ToolSO toolSO = @event.ToolSO;
        itemNameTxt.text = toolSO.toolName;
        pageTxt.text = "인벤토리(" + @event.CurrentPage + "/" + @event.TotalPage + ")";
    }

    private void OnInventoryToggle(InventoryToggleEvent @event)
    {
        if (@event.IsOpen)
        {
            inventoryCanvasGroup.DOFade(1f, 0.5f).SetUpdate(true);
        }
        else
        {
            inventoryCanvasGroup.DOFade(0f, 0.5f).SetUpdate(true);
        }   
    }
}
