using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(StateSO))]
public class StateSOEditor : Editor
{
    [SerializeField] private VisualTreeAsset stateView = default;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        stateView.CloneTree(root);

        DropdownField dropdownField = root.Q<DropdownField>("ClassDropdownField");

        CreateDropDownList(dropdownField);

        dropdownField.RegisterValueChangedCallback(HandleDropdownFieldChange);
        return root;
    }

    private void HandleDropdownFieldChange(ChangeEvent<string> evt)
    {
        StateSO targetData = target as StateSO;
        targetData.className = evt.newValue;

        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
    }
    //aa
    private void CreateDropDownList(DropdownField targetField)
    {
        targetField.choices.Clear();
        //AgentState를 포함하는 어셈블리 가져오기
        Assembly stateAssembly = Assembly.GetAssembly(typeof(AgentState));

        List<Type> derivedTypes = stateAssembly.GetTypes()
            .Where(type => type.IsClass
                           && type.IsAbstract == false
                           && type.IsSubclassOf(typeof(AgentState)))
            .ToList();

        derivedTypes.ForEach(type => targetField.choices.Add(type.FullName));

        if (targetField.choices.Count > 0)
            targetField.SetValueWithoutNotify(derivedTypes[0].FullName);
    }
}
