using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
public class ToolHandlerModule : MonoBehaviour, IModule
{
    [Header("Settings")]
    [SerializeField] private EventChannelSO toolInfoCallEventChannel;

    [SerializeField] private float toolDetectiveRadius = 1.0f;
    [SerializeField] private LayerMask toolLayer;

    [Header("State")]
    private ConfigurableJoint _joint;
    private Tool _lastDetectedTool; // 'Selected'보다 'Detected'가 탐색 의미에 더 적합합니다.

    public Tool CurrentlyEquipedTool { get; private set; }
    public bool IsToolEquiped => CurrentlyEquipedTool != null; // 프로퍼티를 통해 상태 관리

    public void Initialize(ModuleOwner owner)
    {
        _joint = GetComponent<ConfigurableJoint>();
    }

    private void Update()
    {
        // 이미 도구를 들고 있다면 주변 탐색을 하지 않음
        if (IsToolEquiped) return;

        ScanForTools();
    }

    private void ScanForTools()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, toolDetectiveRadius, toolLayer);

        Tool foundTool = null;
        float distance = -1f;
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Tool>(out var tool))
            {
                float exDistance = Vector3.Distance(gameObject.transform.position, hit.transform.position);
                if (exDistance < distance || distance == -1f)
                {
                    distance = exDistance;
                    foundTool = tool;
                }
            }
        }

        if (foundTool == null)
        {
            ClearDetectedTool();
            return;
        }

        if (_lastDetectedTool == foundTool) return;

        ClearDetectedTool();
        _lastDetectedTool = foundTool;
        _lastDetectedTool.ShowToolLabel();
    }

    private void ClearDetectedTool()
    {
        if (_lastDetectedTool != null)
        {
            _lastDetectedTool.HideToolLabel();
            _lastDetectedTool = null;
        }
    }
    //public void EquipTool()
    //{
    //    if (_lastDetectedTool == null || IsToolEquiped) return;

    //    CurrentlyEquipedTool = _lastDetectedTool;

    //    _joint.connectedBody = CurrentlyEquipedTool.GetRigidbody();
    //    CurrentlyEquipedTool.EquipTool();

    //    toolInfoCallEventChannel.RaiseEvent(new ToolEquipEvent(CurrentlyEquipedTool.gameObject.transform, CurrentlyEquipedTool.toolSO));
    //    _lastDetectedTool = null;
    //}
    public void EquipTool()
    {
        if (_lastDetectedTool == null || IsToolEquiped) return;

        HandleEquipTool(_lastDetectedTool);
    }


    public void HandleEquipTool(Tool tool)
    {
        CurrentlyEquipedTool = tool;

        _joint.connectedBody = CurrentlyEquipedTool.GetRigidbody();
        CurrentlyEquipedTool.EquipTool();

        toolInfoCallEventChannel.RaiseEvent(new ToolEquipEvent(CurrentlyEquipedTool.gameObject.transform, CurrentlyEquipedTool.toolSO));
        _lastDetectedTool = null;
    }

    public void UnEquipTool()
    {
        if (!IsToolEquiped) return;

        // 해제 로직
        CurrentlyEquipedTool.UnEquipTool();
        _joint.connectedBody = null;

        // 이벤트 알림
        toolInfoCallEventChannel.RaiseEvent(new ToolUnEquipEvent());

        CurrentlyEquipedTool = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsToolEquiped ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, toolDetectiveRadius);
    }
    public void SaveToolInventory()
    {
        if (!IsToolEquiped) return;

        toolInfoCallEventChannel.RaiseEvent(new ToolSaveToInventoryEvent(CurrentlyEquipedTool));
    }
}