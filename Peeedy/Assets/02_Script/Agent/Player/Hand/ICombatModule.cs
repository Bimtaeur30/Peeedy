using UnityEngine;

public interface ICombatModule : IModule
{
    void Aim(Vector3 worldPosition);
    void Attack();
}
