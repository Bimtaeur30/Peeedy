using UnityEngine;

public class MoneyManager : MonoSingleton<MoneyManager>
{
    [SerializeField] private EventChannelSO moneyEvent;
    private int currentMoney = 0;
    public int CurrentMoney
    {
        get { return currentMoney; }
        set
        {
            currentMoney = value;
            moneyEvent.RaiseEvent(new MoneyEvent(value));
            Debug.Log("현금 잔액이 " + value + "로 변경되었습니다.");
        }
    }
}
