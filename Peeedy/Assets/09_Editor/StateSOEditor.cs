using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(StateSO))]
public class StateSOEditor : UnityEditor.Editor
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
        StateSO targetData = target as StateSO; // 현재 편집중인 so
        targetData.className = evt.newValue; // 드롭다운 값 반영

        EditorUtility.SetDirty(targetData); // 변경됨 표시
        AssetDatabase.SaveAssets(); // 디스크에 저장
    }

    private void CreateDropDownList(DropdownField targetField)
    {
        targetField.choices.Clear();
        Assembly stateAssembly = Assembly.GetAssembly(typeof(AgentState));

        List<Type> derivedTypes = stateAssembly.GetTypes()
            .Where(type => type.IsClass && type.IsAbstract == false && type.IsSubclassOf(typeof(AgentState))).ToList();

        derivedTypes.ForEach(type => targetField.choices.Add(type.FullName));

        if (targetField.choices.Count > 0)
            targetField.SetValueWithoutNotify(derivedTypes[0].FullName);
    }
}
