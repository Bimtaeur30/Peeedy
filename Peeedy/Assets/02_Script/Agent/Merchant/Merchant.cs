using System;
using System.Runtime.InteropServices;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Merchant : Agent
{
    [SerializeField] private string targetSceneName;
    [SerializeField] private BuildingSO buildingSO;
    [SerializeField] private EventChannelSO buildingEnterEvent;
    [SerializeField] private PlayerInputSO playerInputSO;
    [field: SerializeField] public EventChannelSO UIChannel { get; private set; }


    private ChatHandlerModule _chatModule;
    private bool _canEnter = false;

    private void OnEnable()
    {
        playerInputSO.OnBuildingEnterEvent += HandleBuildingEnter;
    }

    private void HandleBuildingEnter()
    {
        if (_canEnter)
        {
            _chatModule.NewChat("상점에 입장하셨습니다.");
            UIChannel.RaiseEvent(UIEvents.FadeEvent.Init(true, 1f, () =>
            {
                SceneManager.LoadScene(targetSceneName);
            }));
        }
    }

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
            _canEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _chatModule.NewChat("다음에 또오세요!");
            buildingEnterEvent.RaiseEvent(new BuildingEnterEvent(buildingSO, false));
            _canEnter = false;
        }
    }
}
