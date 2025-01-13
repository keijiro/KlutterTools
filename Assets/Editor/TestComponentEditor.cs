using UnityEngine;
using UnityEditor;
using KlutterTools.InspectorUtils;

[CustomEditor(typeof(TestComponent))]
public sealed class TestComponentEditor : Editor
{
    AutoProperty _param1 = null;
    AutoProperty _param2 = null;
    AutoProperty _param3 = null;
    AutoProperty _param4 = null;

    static class Labels
    {
        public static LabelString _param1 = "Parameter 1";
        public static LabelString _param2 = "Parameter 2";
        public static LabelString _param3 = "Parameter 3";
        public static LabelString _param4 = "Parameter 4";
    }

    void OnEnable()
      => AutoProperty.Scan(this);

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_param1, Labels._param1);
        EditorGUILayout.PropertyField(_param2, Labels._param2);
        EditorGUILayout.PropertyField(_param3, Labels._param3);
        EditorGUILayout.PropertyField(_param4, Labels._param4);
        serializedObject.ApplyModifiedProperties();
    }
}
