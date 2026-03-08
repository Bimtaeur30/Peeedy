using System.Collections.Generic;
using UnityEngine;

public class OverlapTrigger : MonoBehaviour
{
    public enum Shape
    {
        Box,
        Sphere,
        Capsule
    }

    [Header("Shape Settings")]
    public Shape shape;

    public Vector3 boxHalfExtents = Vector3.one;
    public float sphereRadius = 1f;

    public float capsuleRadius = 1f;
    public float capsuleHeight = 2f;

    [Header("Detection")]
    public LayerMask layerMask;

    private Collider[] _buffer = new Collider[32];

    private readonly HashSet<Collider> _current = new();
    private readonly HashSet<Collider> _previous = new();

    void FixedUpdate()
    {
        CheckOverlap();
    }

    void CheckOverlap()
    {
        _previous.Clear();
        foreach (var col in _current)
            _previous.Add(col);

        _current.Clear();

        int count = GetOverlap();

        for (int i = 0; i < count; i++)
            _current.Add(_buffer[i]);

        DetectEnterExit();
    }

    int GetOverlap()
    {
        switch (shape)
        {
            case Shape.Box:
                return Physics.OverlapBoxNonAlloc(
                    transform.position,
                    boxHalfExtents,
                    _buffer,
                    transform.rotation,
                    layerMask);

            case Shape.Sphere:
                return Physics.OverlapSphereNonAlloc(
                    transform.position,
                    sphereRadius,
                    _buffer,
                    layerMask);

            case Shape.Capsule:

                Vector3 point1 = transform.position + Vector3.up * (capsuleHeight * 0.5f - capsuleRadius);
                Vector3 point2 = transform.position + Vector3.down * (capsuleHeight * 0.5f - capsuleRadius);

                return Physics.OverlapCapsuleNonAlloc(
                    point1,
                    point2,
                    capsuleRadius,
                    _buffer,
                    layerMask);
        }

        return 0;
    }

    void DetectEnterExit()
    {
        foreach (var col in _current)
        {
            if (!_previous.Contains(col))
                OnTriggerEnterOverlap(col);
        }

        foreach (var col in _previous)
        {
            if (!_current.Contains(col))
                OnTriggerExitOverlap(col);
        }
    }

    protected virtual void OnTriggerEnterOverlap(Collider other)
    {
        Debug.Log("Enter : " + other.name);
    }

    protected virtual void OnTriggerExitOverlap(Collider other)
    {
        Debug.Log("Exit : " + other.name);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        switch (shape)
        {
            case Shape.Box:
                Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2);
                break;

            case Shape.Sphere:
                Gizmos.DrawWireSphere(Vector3.zero, sphereRadius);
                break;

            case Shape.Capsule:

                Vector3 top = Vector3.up * (capsuleHeight * 0.5f - capsuleRadius);
                Vector3 bottom = Vector3.down * (capsuleHeight * 0.5f - capsuleRadius);

                Gizmos.DrawWireSphere(top, capsuleRadius);
                Gizmos.DrawWireSphere(bottom, capsuleRadius);

                break;
        }
    }
}
