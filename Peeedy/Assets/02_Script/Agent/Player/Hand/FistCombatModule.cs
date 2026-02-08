using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class FistCombatModule : MonoBehaviour, ICombatModule
{
    [SerializeField] private Transform pivot;
    [SerializeField] private SpriteRenderer hand;
    private ModuleOwner _owner;

    public void Initialize(ModuleOwner owner)
    {
        _owner = owner;
    }

    public void Aim(Vector3 worldPosition)
    {

        Vector3 dir = worldPosition - pivot.position;

        if (dir.sqrMagnitude < 0.0001f)
            return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (angle > -90 && angle < 90)
            hand.flipY = true;
        else
            hand.flipY = false;

        pivot.rotation = Quaternion.Euler(0f, 0f, angle + 180);
    }

    public void Attack()
    {
        Debug.Log("АјАн!");
    }
}
