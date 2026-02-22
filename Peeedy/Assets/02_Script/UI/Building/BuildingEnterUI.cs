using DG.Tweening;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class BuildingEnterUI : MonoBehaviour
{
    [SerializeField] private EventChannelSO buildingEnterEvent;
    [SerializeField] private RectTransform pannelRec;
    [SerializeField] private TextMeshProUGUI buildingTxt;
    [SerializeField] private float hidePosY;
    [SerializeField] private float showPosY;
    [SerializeField] private float animDuration = 1.0f;
    [SerializeField] private Ease animEase;

    private void OnEnable()
    {
        buildingEnterEvent.AddListener<BuildingEnterEvent>(OnEventUI);
    }


    private void OnEventUI(BuildingEnterEvent so)
    {
        if (so.IsEnter)
        {
            buildingTxt.text = so.BuildingSO.BuildingName;
            pannelRec.DOAnchorPosY(showPosY, animDuration).SetEase(animEase);
        }
        else
        {
            pannelRec.DOAnchorPosY(hidePosY, animDuration);
        }
    }
}
