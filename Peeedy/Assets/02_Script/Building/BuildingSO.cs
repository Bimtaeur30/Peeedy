using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "Building/BuildingSO")]
public class BuildingSO : ScriptableObject
{
    [field:SerializeField] public string BuildingName { get; private set; }
}
