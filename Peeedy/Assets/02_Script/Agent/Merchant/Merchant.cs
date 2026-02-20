using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class Merchant : Agent
{
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _chatModule.NewChat("다음에 또오세요!");
        }
    }
}
