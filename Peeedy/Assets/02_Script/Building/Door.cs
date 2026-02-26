using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DoorType
{
    EnterDoor,
    ExitDoor
}

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour
{
    [SerializeField] private DoorType doorType;
    [SerializeField] private string targetSceneName;

    [SerializeField] private BuildingSO buildingSO;
    [SerializeField] private PlayerInputSO playerInputSO;

    [SerializeField] private EventChannelSO buildingEnterEvent;
    [SerializeField] private EventChannelSO uiChannel;
    [SerializeField] private EventChannelSO systemChannel;

    private bool _canEnter = false;

    private void OnEnable()
    {
        playerInputSO.OnBuildingEnterEvent += HandleBuildingEnter;
    }

    private void OnDisable()
    {
        playerInputSO.OnBuildingEnterEvent -= HandleBuildingEnter;
    }

    private void HandleBuildingEnter()
    {
        if (_canEnter)
        {
            uiChannel.RaiseEvent(UIEvents.FadeEvent.Init(true, 1f, () =>
            {
                systemChannel.RaiseEvent(SystemEvents.savePrefEvent);
                SceneManager.LoadScene(targetSceneName);

            }));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            buildingEnterEvent.RaiseEvent(new DoorTriggerEvent(doorType, buildingSO, true));
            _canEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            buildingEnterEvent.RaiseEvent(new DoorTriggerEvent(doorType, buildingSO, false));
            _canEnter = false;
        }
    }
}
