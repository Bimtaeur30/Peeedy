using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
public class ToolHandlerModule : MonoBehaviour, IModule
{
    [SerializeField] private EventChannelSO toolInfoCallEventChannel;
    [SerializeField] private float toolDetectiveRadius = 1.0f;
    [SerializeField] private LayerMask toolLayer;
    private ConfigurableJoint _joint;
    private Tool _lastSelectedTool;

    public bool IsToolEquiped { get; private set; } // 현재 장착한 툴이 있는가

    public void Initialize(ModuleOwner owner)
    {
        _joint = GetComponent<ConfigurableJoint>();
    }

    private void Update()
    {
        if (IsToolEquiped) return; // 현재 장착중인 도구가 있다면 검사하지 않음

        Collider[] hits = Physics.OverlapSphere(transform.position, toolDetectiveRadius, toolLayer);
        if (hits.Length == 0 )
        {
            _lastSelectedTool?.HideToolLabel();
            _lastSelectedTool = null;
            return;
        }
        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<Tool>(out Tool tool)) // 리지드바디 말고 툴 스크립트로 교체하기
            {
                if (_lastSelectedTool == tool) return;// 전에 선택된거랑 같은 툴이라면 스킵
                else // 다르다면 선택 UI꺼주고 새로운 툴로 갱신해주기
                {
                    _lastSelectedTool?.HideToolLabel();
                    _lastSelectedTool = tool;
                    _lastSelectedTool.ShowToolLabel();
                    Debug.Log("우헤헿");
                }
            }
        }
    }

    public void EquipTool()
    {
        if (_lastSelectedTool == null) return;
        Rigidbody rigidbody = _lastSelectedTool.GetRigidbody();

        _joint.connectedBody = rigidbody;
        toolInfoCallEventChannel.RaiseEvent(new ToolEquipEvent());
        IsToolEquiped = true;
    }

    public void UnEquipTool()
    {
        _joint.connectedBody = null;
        _lastSelectedTool = null;
        toolInfoCallEventChannel.RaiseEvent(new ToolUnEquipEvent());
        IsToolEquiped = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, toolDetectiveRadius);
    }
}
