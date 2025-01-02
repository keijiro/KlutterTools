using System;
using System.Reflection;
using System.Collections.ObjectModel;
using UnityEditor;
using UnityEngine;

namespace Klak.EditorTools {

[InitializeOnLoad]
public static class VfxGraphExtraPreference
{
    static VfxGraphExtraPreference()
      => EditorWindow.windowFocusChanged += OnWindowFocusChanged;

    public const string ZoomStepScaleKey = "VFX.Extra.ZoomStepScale";

    public static float ZoomStepScale
      { get => EditorPrefs.GetFloat(ZoomStepScaleKey, 1);
        set => EditorPrefs.SetFloat(ZoomStepScaleKey, value); }

    public static void OnWindowFocusChanged()
    {
        var target = EditorWindow.focusedWindow;

        var vfxViewWindowType = Type.GetType
          ("UnityEditor.VFX.UI.VFXViewWindow, Unity.VisualEffectGraph.Editor");

        if (!vfxViewWindowType.IsInstanceOfType(target)) return;

        var graphViewProperty = vfxViewWindowType.GetProperty
          ("graphView",
           BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

        var graphView = graphViewProperty.GetValue(target);
        if (graphView == null) return;

        var vfxViewType = graphView.GetType();
        var argTypes = new []
          { typeof(float), typeof(float), typeof(float), typeof(float) };

        var setupZoomMethod = vfxViewType.GetMethod
          ("SetupZoom",
           BindingFlags.Public | BindingFlags.Instance, null, argTypes, null);

        var args = new object[] { 0.125f, 8, 0.15f * ZoomStepScale, 1};
        setupZoomMethod.Invoke(graphView, args);
    }
}

class VfxGraphExtraPreferenceProvider : SettingsProvider
{
    public VfxGraphExtraPreferenceProvider()
      : base("Preferences/Visual Effects Extra", SettingsScope.User) {}

    public override void OnGUI(string search)
    {
        EditorGUI.BeginChangeCheck();

        var scale = VfxGraphExtraPreference.ZoomStepScale;
        scale = EditorGUILayout.FloatField("Zoom Step Scale", scale);

        if (EditorGUI.EndChangeCheck())
            VfxGraphExtraPreference.ZoomStepScale = scale;

        base.OnGUI(search);
    }

    [SettingsProvider]
    public static SettingsProvider PreferenceSettingsProvider()
      => new VfxGraphExtraPreferenceProvider();
}

} // namespace Klak.EditorTools 
