using UnityEngine;

[CreateAssetMenu(fileName = "ToolSO", menuName = "Tool/ToolSO")]
public class ToolSO : ScriptableObject
{
    [field:SerializeField] public string toolName { get; private set; }
    [field: SerializeField] public int toolDetectRange { get; private set; } = 2;
    [field: SerializeField] public DummyMessageSO messageSO { get; private set; }
}
