using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainPanelUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _root;
    private VisualElement _popUpWindow;
    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        _root = _uiDocument.rootVisualElement;
        VisualElement topContainer = _root.Q<VisualElement>("TopContainer");

        topContainer.RegisterCallback<ClickEvent>(HandleButtonClickEvent);
        _popUpWindow = _root.Q<VisualElement>("PopUpWindow");
    }
    private void HandleButtonClickEvent(ClickEvent evt)
    {
        if (evt.target is ButtonData { ButtonIndex : 1} dataButton)
        {
            OpenPopUpWindow();
        }
    }

    private void OpenPopUpWindow()
    {
        _popUpWindow.AddToClassList("open");
    }
}
