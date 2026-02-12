using System.Collections.Generic;
using UnityEngine;

public class ToolRangeVisualizer : MonoBehaviour
{
    [SerializeField] EventChannelSO toolInfoCallEventChannel;
    [SerializeField] LayerMask dummyLayer;
    [SerializeField] GameObject visualizerObj;
    [SerializeField] float detectRadius = 1f;

    bool _isToolEquiped;
    Transform _equipedToolTran;
    ToolSO _currentToolSO;

    readonly HashSet<Dummy> _inRange = new HashSet<Dummy>();
    readonly HashSet<Dummy> _thisFrame = new HashSet<Dummy>();

    private void OnEnable()
    {
        toolInfoCallEventChannel.AddListener<ToolEquipEvent>(HandleToolEquipEvt);
        toolInfoCallEventChannel.AddListener<ToolUnEquipEvent>(HandleToolUnEquipEvt);
    }

    private void OnDisable()
    {
        toolInfoCallEventChannel.RemoveListener<ToolEquipEvent>(HandleToolEquipEvt);
        toolInfoCallEventChannel.RemoveListener<ToolUnEquipEvent>(HandleToolUnEquipEvt);

        // 꺼질 때는 범위 안에 있던 더미들 모두 정지
        StopAllDummies();
    }

    private void Update()
    {
        if (!_isToolEquiped) return;

        // 툴 위치 따라가기
        transform.position = new Vector3(_equipedToolTran.position.x, 0, _equipedToolTran.position.z);

        // 1) 이번 프레임 더미 수집
        _thisFrame.Clear();
        var cols = Physics.OverlapSphere(transform.position, detectRadius, dummyLayer);
        foreach (var c in cols)
        {
            // Dummy가 자식에 붙을 수도 있으니 parent까지 탐색
            var dummy = c.GetComponentInParent<Dummy>();
            if (dummy != null) _thisFrame.Add(dummy);
        }

        // 2) Enter 처리: 이번 프레임엔 있는데, 이전엔 없던 애들
        foreach (var d in _thisFrame)
        {
            if (_inRange.Add(d))
            {
                d.EnterToolRange(_currentToolSO);
            }
        }

        // 3) Exit 처리: 이전엔 있었는데, 이번 프레임엔 없는 애들
        // HashSet 순회 중 삭제가 안 되니 임시 리스트 사용
        if (_inRange.Count > 0)
        {
            _toRemove.Clear();
            foreach (var d in _inRange)
                if (!_thisFrame.Contains(d))
                    _toRemove.Add(d);

            foreach (var d in _toRemove)
            {
                _inRange.Remove(d);
                if (d != null) d.ExitToolRange();
            }
        }
    }

    readonly List<Dummy> _toRemove = new List<Dummy>();

    void HandleToolEquipEvt(ToolEquipEvent evt)
    {
        _isToolEquiped = true;
        _equipedToolTran = evt.ToolPosition;
        _currentToolSO = evt.ToolSO;

        visualizerObj.SetActive(true);

        //detectRadius = Mathf.Max(0.1f, _currentToolSO != null ? _currentToolSO.toolDetectRange : 1f);

        // 이미 범위 안에 있던 더미가 있을 수 있으니, 다음 Update에서 Enter가 잡히게끔 초기화
        _inRange.Clear();
    }

    void HandleToolUnEquipEvt(ToolUnEquipEvent evt)
    {
        _isToolEquiped = false;
        visualizerObj.SetActive(false);

        StopAllDummies();

        _currentToolSO = null;
        _equipedToolTran = null;
    }

    void StopAllDummies()
    {
        foreach (var d in _inRange)
            if (d != null) d.ExitToolRange();

        _inRange.Clear();
        _thisFrame.Clear();
        _toRemove.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
