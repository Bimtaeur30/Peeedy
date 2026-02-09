using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ToolInfoViewer : MonoBehaviour
{
    [SerializeField] private EventChannelSO toolInfoCallEventChannel;
    [SerializeField] private GameObject toolInfoLabelUI;
    [SerializeField] private TextMeshProUGUI toolNameTxt;
    [SerializeField] private TextMeshProUGUI keyTxt;

    private bool _isInfoActivate;
    private Transform _currentSelectedTool;

    private void OnEnable()
    {
        toolInfoCallEventChannel.AddListener<ToolInfoCallEvent>(HandleToolInfoCallEvt);
        toolInfoCallEventChannel.AddListener<ToolEquipEvent>(HandleToolEquipEvt);
        toolInfoCallEventChannel.AddListener<ToolUnEquipEvent>(HandleToolunEquipEvt);
    }
    private void OnDisable()
    {
        toolInfoCallEventChannel.RemoveListener<ToolInfoCallEvent>(HandleToolInfoCallEvt);
        toolInfoCallEventChannel.RemoveListener<ToolEquipEvent>(HandleToolEquipEvt);
        toolInfoCallEventChannel.RemoveListener<ToolUnEquipEvent>(HandleToolunEquipEvt);
    }
    private void Update()
    {
        if (_isInfoActivate)
        {
            transform.position = _currentSelectedTool.position + new Vector3(0, 0, -0.01f); // 겹침 방지
        }
    }

    private void HandleToolInfoCallEvt(ToolInfoCallEvent evt)
    {
        _isInfoActivate = evt.IsActivate;
        if (evt.IsActivate)
        {
            toolInfoLabelUI.gameObject.SetActive(true);
            toolNameTxt.text = evt.ToolSO.toolName;
            _currentSelectedTool = evt.ToolPosition;
        }
        else
        {
            toolInfoLabelUI.gameObject.SetActive(false);
        }
    }

    private void HandleToolEquipEvt(ToolEquipEvent evt)
    {
        keyTxt.text = "Q";
        toolNameTxt.text = "내려놓기";
    }
    private void HandleToolunEquipEvt(ToolUnEquipEvent evt)
    {
        keyTxt.text = "E";
    }
}
