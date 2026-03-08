using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Merchant : Agent
{

    [SerializeField] private string[] messages;
    [SerializeField] private float messageDelay;
    private ChatHandlerModule _chatModule;
    protected override void AfterInitComponents()
    {
        base.AfterInitComponents();
        _chatModule = GetModule<ChatHandlerModule>();
    }

    private void Start()
    {
        StartCoroutine(Message());
    }

    IEnumerator Message()
    {
        int index = 0;

        while(true)
        {
            _chatModule.NewChat(messages[UnityEngine.Random.Range(0, messages.Length - 1)]);
            yield return new WaitForSeconds(messageDelay);
        }
    }

    //[SerializeField] private string targetSceneName;
    //[SerializeField] private BuildingSO buildingSO;
    //[SerializeField] private EventChannelSO buildingEnterEvent;
    //[SerializeField] private PlayerInputSO playerInputSO;
    //[field: SerializeField] public EventChannelSO UIChannel { get; private set; }


    //private ChatHandlerModule _chatModule;
    //private bool _canEnter = false;

    //private void OnEnable()
    //{
    //    playerInputSO.OnBuildingEnterEvent += HandleBuildingEnter;
    //}

    //private void HandleBuildingEnter()
    //{
    //    if (_canEnter)
    //    {
    //        _chatModule.NewChat("상점에 입장하셨습니다.");
    //        UIChannel.RaiseEvent(UIEvents.FadeEvent.Init(true, 1f, () =>
    //        {
    //            SceneManager.LoadScene(targetSceneName);
    //        }));
    //    }
    //}

    //protected override void AfterInitComponents()
    //{
    //    base.AfterInitComponents();
    //    _chatModule = GetModule<ChatHandlerModule>();
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        _chatModule.NewChat("어서오시오~");
    //        buildingEnterEvent.RaiseEvent(new DoorTriggerEvent(buildingSO, true));
    //        _canEnter = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        _chatModule.NewChat("다음에 또오세요!");
    //        buildingEnterEvent.RaiseEvent(new DoorTriggerEvent(buildingSO, false));
    //        _canEnter = false;
    //    }
    //}
}
