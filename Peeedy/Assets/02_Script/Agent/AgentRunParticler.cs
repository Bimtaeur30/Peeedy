using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AgentRunParticler : MonoBehaviour, IModule
{
    private ParticleSystem _system;
    
    public void Initialize(ModuleOwner owner)
    {
        _system = GetComponent<ParticleSystem>();
    }

    public void PlayParticle()
    {
        _system.Play();
    }
    public void StopParticle()
    {
        _system.Stop();
    }
}
