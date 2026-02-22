using UnityEngine;

public class Merchant : Agent
{
    [SerializeField] private BuildingSO buildingSO;
    [SerializeField] private EventChannelSO buildingEnterEvent;
    private ChatHandlerModule _chatModule;
    
    protected override void AfterInitComponents()
    {
        base.AfterInitComponents();
        _chatModule = GetModule<ChatHandlerModule>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _chatModule.NewChat("어서오시오~");
            buildingEnterEvent.RaiseEvent(new BuildingEnterEvent(buildingSO, true));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _chatModule.NewChat("다음에 또오세요!");
            buildingEnterEvent.RaiseEvent(new BuildingEnterEvent(buildingSO, false));
        }
    }
}
