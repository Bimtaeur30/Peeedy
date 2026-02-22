using System;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyTxt;
    [SerializeField] private EventChannelSO moneyEvent;

    private void OnEnable()
    {
        moneyEvent.AddListener<MoneyEvent>(OnEvent);
    }

    private void OnEvent(MoneyEvent @event)
    {
        moneyTxt.text = "ภÜพื: "+@event.MoneyAmount.ToString();
    }
}
