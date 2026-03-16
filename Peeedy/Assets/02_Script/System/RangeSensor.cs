using UnityEngine;

public abstract class RangeSensor : MonoBehaviour
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask layerMask;
    private Collider detectObj = null;

    private void Update()
    {
        Collider obj = Sensor();

        if (obj != detectObj)
        {
            if (detectObj != null)
                OnUnDetected(detectObj.gameObject.transform.root.gameObject);

            if (obj != null)
                OnDetected(obj.gameObject.transform.root.gameObject);

            detectObj = obj;
        }
    }

    protected abstract void OnDetected(GameObject obj);
    protected abstract void OnUnDetected(GameObject obj);

    private Collider Sensor()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerMask);

        float minDistance = float.MaxValue;
        Collider obj = default;

        foreach(Collider collider in colliders)
        {
            float distance = Vector3.Distance(collider.gameObject.transform.position, transform.position);
            if (minDistance > distance)
            {
                minDistance = distance;
                obj = collider;
            }
        }

        if (obj != null)
            return obj;
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
