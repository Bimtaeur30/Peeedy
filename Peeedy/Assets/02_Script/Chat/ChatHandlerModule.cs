using System.Collections;
using UnityEngine;

public class ChatHandlerModule : MonoBehaviour, IModule
{
    [SerializeField] private Chat chatPrefab;
    [SerializeField] private float chatLiftTime = 1.5f;
    [SerializeField] private AudioClip[] dummyVoiceSfxs;

    Chat _currentChat;
    private Coroutine _chatTimerCoroutine;
    private AudioSource _audioSource;
    public void Initialize(ModuleOwner owner)
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void NewChat(string message)
    {
        _currentChat?.Close();

        _audioSource.PlayOneShot(dummyVoiceSfxs[Random.Range(0, dummyVoiceSfxs.Length - 1)]);
        _currentChat = Instantiate(chatPrefab, transform);
        _currentChat.Setup(message);

        if (_chatTimerCoroutine != null)
            StopCoroutine(_chatTimerCoroutine);

        _chatTimerCoroutine = StartCoroutine(ChatTimer());
    }


    IEnumerator ChatTimer()
    {
        yield return new WaitForSeconds(chatLiftTime);
        _currentChat?.Close();
        _currentChat = null;
    }

    //IEnumerator AA()
    //{
    //    while(true)
    //    {
    //        yield return new WaitForSeconds(1f);
    //        NewChat("달을 향해 쏴라 그럼 별이 될테니");
    //    }
    //}
}
