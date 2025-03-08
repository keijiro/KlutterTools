using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace KlutterTools.InspectorUtils {

// String container providing GUIContent
public struct LabelString
{
    GUIContent _guiContent;

    public static implicit operator GUIContent(LabelString label)
      => label._guiContent;

    public static implicit operator LabelString(string text)
      => new LabelString { _guiContent = new GUIContent(text) };
}

// Auto-scanning serialized property wrapper
public struct AutoProperty
{
    SerializedProperty _prop;

    public SerializedProperty Target
      => _prop;

    public AutoProperty(SerializedProperty prop)
      => _prop = prop;

    public static implicit operator AutoProperty(SerializedProperty prop)
      => new AutoProperty(prop);

    public static implicit operator SerializedProperty(AutoProperty prop)
      => prop._prop;

    public static void Scan<T>(T target) where T : UnityEditor.Editor
    {
        var so = target.serializedObject;

        var allFields = typeof(T).GetFields
          (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        var autoProps = allFields.Where(f => f.FieldType == typeof(AutoProperty));

        foreach (var f in autoProps)
            f.SetValue(target, new AutoProperty(FindProperty(so, ResolveNesting(f.Name))));
    }

    static string ResolveNesting(string name)
      => name.Replace("__", ".");

    static string GetPropNameBacking(string name)
      => $"<{name}>k__BackingField";

    static string GetPropNameCamel(string name)
      => name.TrimStart('_');

    static string GetPropNamePascal(string name)
    {
        var trimmed = name.TrimStart('_');
        return char.ToUpper(trimmed[0]) + trimmed.Substring(1);
    }

    static SerializedProperty FindProperty(SerializedObject so, string name)
      => so.FindProperty(name) ??
         so.FindProperty(GetPropNameCamel(name)) ??
         so.FindProperty(GetPropNamePascal(name)) ??
         so.FindProperty(GetPropNameBacking(name)) ??
         so.FindProperty(GetPropNameBacking(GetPropNameCamel(name))) ??
         so.FindProperty(GetPropNameBacking(GetPropNamePascal(name)));
}

} // namespace KlutterTools.InspectorUtils
