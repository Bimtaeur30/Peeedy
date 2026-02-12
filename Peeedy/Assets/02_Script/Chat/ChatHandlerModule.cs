using System.Collections;
using UnityEngine;

public class ChatHandlerModule : MonoBehaviour, IModule
{
    [SerializeField] private Chat chatPrefab;
    [SerializeField] private float chatLiftTime = 1.5f;
    Chat _currentChat;
    private Coroutine _chatTimerCoroutine;
    public void Initialize(ModuleOwner owner)
    {
    }

    public void NewChat(string message)
    {
        _currentChat?.Close();

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
