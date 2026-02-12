using System.Runtime.InteropServices;
using UnityEngine;

public class AgentSensor : MonoBehaviour, IModule
{
    [SerializeField] private Vector3 sensorSize;

    public void Initialize(ModuleOwner owner) { }

    public GameObject GetCurrentDetectObj(LayerMask layer)
    {
        Collider[] hits = Physics.OverlapBox(transform.position, sensorSize, Quaternion.identity, layer);
        foreach(Collider hit in hits)
        {
            return hit.gameObject;
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, sensorSize);
    }
}
