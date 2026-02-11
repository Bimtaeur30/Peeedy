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

    private bool _isEquiped = false; // 장착 상태 추적

    private void HandleToolInfoCallEvt(ToolInfoCallEvent evt)
    {
        // 이미 장착 중이라면 바닥에 있는 다른 도구의 Label 요청은 무시합니다.
        if (_isEquiped) return;

        _isInfoActivate = evt.IsActivate;
        toolInfoLabelUI.gameObject.SetActive(evt.IsActivate);

        if (evt.IsActivate)
        {
            toolNameTxt.text = evt.ToolSO.toolName;
            keyTxt.text = "E"; // 기본 줍기 키
            _currentSelectedTool = evt.ToolPosition;
        }
    }

    private void HandleToolEquipEvt(ToolEquipEvent evt)
    {
        _isEquiped = true;
        _isInfoActivate = true; // 장착 중에도 위치 업데이트는 계속 수행

        toolInfoLabelUI.gameObject.SetActive(true); // 혹시 꺼졌다면 다시 킴
        keyTxt.text = "Q";
        toolNameTxt.text = "내려놓기";
    }

    private void HandleToolunEquipEvt(ToolUnEquipEvent evt)
    {
        _isEquiped = false;
        toolInfoLabelUI.gameObject.SetActive(false); // 내려놓으면 일단 UI 끔
    }
}
