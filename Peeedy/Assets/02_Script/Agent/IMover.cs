using System;
using UnityEngine;

public interface IMover
{
    bool CanManualMovement { get; set; }
    event Action<Vector3> OnVelocityChange;

    void SetMoveSpeedMultiplier(float value);
    void AddForceToAgent(Vector3 force);
    void StopImmediately();
    void SetMovement(Vector3 vector); 

}
