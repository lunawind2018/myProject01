using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WS;

[CustomEditor(typeof(MyText))]
public class MyTestEditor : UnityEditor.UI.TextEditor
{
    public override void OnInspectorGUI()
    {
        var textId = serializedObject.FindProperty("textId");
        EditorGUILayout.PropertyField(textId);
        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}
