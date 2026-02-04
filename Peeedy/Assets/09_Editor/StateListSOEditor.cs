using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public static class CodeFormat
{
    public static string EnumFormat =
    @"
    public enum {0}
    {{
        {1}
    }}
    ";
}

[CustomEditor(typeof(StateListSO))]
public class StateListSOEditor : UnityEditor.Editor
{
    [SerializeField] private VisualTreeAsset stateListView = default;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        InspectorElement.FillDefaultInspector(root, serializedObject, this);
        stateListView.CloneTree(root);

        root.Q<Button>("GenerateButton").clicked += HandleGenerateButtonClick;

        return root;
    }

    private void HandleGenerateButtonClick()
    {
        StateListSO list = target as StateListSO;

        int index = 0;
        string enumString = string.Join(",", list.states.Select(so =>
        {
            so.stateIndex = index;
            EditorUtility.SetDirty(so);
            return $"{so.stateName} = {index++}";
        }));

        string scriptPath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(list));
        string dirName = Path.GetDirectoryName(scriptPath);
        DirectoryInfo parentDirectory = Directory.GetParent(dirName);
        string path = parentDirectory.FullName;
        string code = string.Format(CodeFormat.EnumFormat, list.enumName, enumString);

        File.WriteAllText($"{path}/{list.enumName}.cs", code);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
