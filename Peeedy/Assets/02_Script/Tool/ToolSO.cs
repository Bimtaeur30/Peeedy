using UnityEngine;

[CreateAssetMenu(fileName = "ToolSO", menuName = "Tool/ToolSO")]
public class ToolSO : ScriptableObject
{
    [field:SerializeField] public string toolName { get; private set; }
}
