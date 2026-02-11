using System;
using UnityEngine;

public class ToolRangeVisualizer : MonoBehaviour
{
    [SerializeField] private EventChannelSO toolInfoCallEventChannel;
    [SerializeField] private GameObject visualizerObj;
    private bool _isToolEquiped = false;
    private Transform _equipedToolTran;

    private void OnEnable()
    {
        toolInfoCallEventChannel.AddListener<ToolEquipEvent>(HandleToolEquipEvt);
        toolInfoCallEventChannel.AddListener<ToolUnEquipEvent>(HandleToolunEquipEvt);
    }
    private void OnDisable()
    {
        toolInfoCallEventChannel.RemoveListener<ToolEquipEvent>(HandleToolEquipEvt);
        toolInfoCallEventChannel.RemoveListener<ToolUnEquipEvent>(HandleToolunEquipEvt);
    }

    private void Update()
    {
        if (_isToolEquiped)
        {
            gameObject.transform.position = new Vector3(_equipedToolTran.position.x, 0, _equipedToolTran.position.z);
        }
    }

    private void HandleToolEquipEvt(ToolEquipEvent @event)
    {
        _isToolEquiped = true;
        _equipedToolTran = @event.ToolPosition;
        visualizerObj.SetActive(true);
    }

    private void HandleToolunEquipEvt(ToolUnEquipEvent @event)
    {
        _isToolEquiped = false;
        visualizerObj.SetActive(false);
    }
}
