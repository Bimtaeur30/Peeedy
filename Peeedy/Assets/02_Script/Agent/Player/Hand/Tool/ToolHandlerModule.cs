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

    public void EquipTool()
    {
        if (_lastDetectedTool == null || IsToolEquiped) return;

        // 1. 참조 먼저 저장
        CurrentlyEquipedTool = _lastDetectedTool;

        // 2. 물리 조인트 및 사운드 실행
        _joint.connectedBody = CurrentlyEquipedTool.GetRigidbody();
        CurrentlyEquipedTool.EquipTool();

        // 3. [중요] '장착 완료' 이벤트를 먼저 보냅니다.
        // Viewer가 이 이벤트를 먼저 받아서 UI 내용을 "내려놓기(Q)"로 바꿀 수 있게 합니다.
        toolInfoCallEventChannel.RaiseEvent(new ToolEquipEvent(CurrentlyEquipedTool.gameObject.transform));

        // 4. 그 다음 감지용 변수를 비워줍니다.
        // 이때 ClearDetectedTool 내부에 HideToolLabel이 있다면 
        // Viewer에서 "장착 중일 땐 무시"하는 로직이 필요합니다.
        _lastDetectedTool = null;

        Debug.Log("Equip Event Raised Successfully!");
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
}