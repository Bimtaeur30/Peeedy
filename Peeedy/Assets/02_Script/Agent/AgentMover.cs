using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AgentMover : MonoBehaviour, IModule, IMover
{
    [Header("Agent values")]
    [SerializeField] private float moveSpeed = 6f;

    private Agent _owner;
    private Rigidbody _rigidbody;
    private IRenderer _renderer;
    private ParticleSystem _particle;

    private Vector3 _movement;
    private float _moveSpeedMultiplier;

    public event Action<Vector3> OnVelocityChange;
    public bool CanManualMovement { get; set; } = true;


    public void Initialize(ModuleOwner owner)
    {
        _owner = owner as Agent;
        _rigidbody = owner.GetComponent<Rigidbody>();
        _renderer = owner.GetModule<IRenderer>();
        _particle = GetComponent<ParticleSystem>();
        _moveSpeedMultiplier = 1f;
    }

    public void SetMoveSpeedMultiplier(float value) => _moveSpeedMultiplier = value;

    public void AddForceToAgent(Vector3 force)
    {
        _rigidbody.AddForce(force, ForceMode.Impulse);
    }

    public void StopImmediately()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _movement = Vector3.zero;
    }

    public void SetMovement(Vector3 vector)
    {
        _movement = vector;
        _renderer.FlipController(_movement.x);
    }

    public ParticleSystem GetParticle()
    {
        return _particle;
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        if (CanManualMovement)
        {
            _rigidbody.linearVelocity = _movement * moveSpeed * _moveSpeedMultiplier;

        }

        OnVelocityChange?.Invoke(_rigidbody.linearVelocity);
    }
}
