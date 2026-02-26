using System;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyTxt;
    [SerializeField] private EventChannelSO playerChannel;

    private void OnEnable()
    {
        playerChannel.AddListener<MoneyChanged>(OnEvent);
    }

    private void OnEvent(MoneyChanged @event)
    {
        moneyTxt.text = "ภÜพื: "+@event.amount.ToString();
    }
}
