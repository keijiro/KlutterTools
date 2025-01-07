using System;
using System.Reflection;
using UnityEditor;

namespace Klak.KlutterTools {

#if KLUTTER_TOOLS_HAS_VFXGRAPH

[InitializeOnLoad]
public static class VfxGraphZoomStepModifier
{
    static VfxGraphZoomStepModifier()
      => EditorWindow.windowFocusChanged += OnWindowFocusChanged;

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

        var args = new object[]
          { 0.125f, 8, 0.15f * Preferences.VfxGraphZoomStep, 1 };
        setupZoomMethod.Invoke(graphView, args);
    }
}

#endif

} // namespace Klak.KlutterTools 
