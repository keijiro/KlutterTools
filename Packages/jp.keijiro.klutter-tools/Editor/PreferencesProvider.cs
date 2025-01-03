using UnityEditor;
using UnityEngine;

namespace Klak.KlutterTools {

// Preference accessor
public static class Preferences
{
    public static bool FpsCapperEnable
      { get => EditorPrefs.GetBool(Keys.FpsCapperEnable, false);
        set => EditorPrefs.SetBool(Keys.FpsCapperEnable, value); }

    public static int FpsCapperFrameRate
      { get => EditorPrefs.GetInt(Keys.FpsCapperFrameRate, 60);
        set => EditorPrefs.SetInt(Keys.FpsCapperFrameRate, value); }

    public static float VfxGraphZoomStep
      { get => EditorPrefs.GetFloat(Keys.VfxGraphZoomStep, 1);
        set => EditorPrefs.SetFloat(Keys.VfxGraphZoomStep, value); }

    // Preference keys
    static class Keys
    {
        public const string FpsCapperEnable
          = "Klak.KlutterTools.FpsCapper.Enable";

        public const string FpsCapperFrameRate
          = "Klak.KlutterTools.FpsCapper.FrameRate";

        public const string VfxGraphZoomStep
          = "Klak.KlutterTools.VfxGraph.ZoomStep";
    }
}

// Preference provider (GUI)
public sealed class PreferencesProvider : SettingsProvider
{
    public PreferencesProvider()
      : base("Preferences/Klutter Tools", SettingsScope.User) {}

    public override void OnGUI(string search)
    {
        var h = EditorGUIUtility.singleLineHeight / 2;

        GUILayout.BeginHorizontal();
        GUILayout.Space(h);
        GUILayout.BeginVertical();
        GUILayout.Space(h);

        EditorGUI.BeginChangeCheck();

        var fc_enable = Preferences.FpsCapperEnable;
        var fc_frameRate = Preferences.FpsCapperFrameRate;
        var vg_zoomStep = Preferences.VfxGraphZoomStep;

        GUILayout.Label("FPS Capper", EditorStyles.boldLabel);
        fc_enable = EditorGUILayout.Toggle("Enable", fc_enable);
        fc_frameRate = EditorGUILayout.IntField("Target Frame Rate", fc_frameRate);

        GUILayout.Space(h);

        GUILayout.Label("VFX Graph", EditorStyles.boldLabel);
        vg_zoomStep = EditorGUILayout.FloatField("Zoom Step Scale", vg_zoomStep);

        if (EditorGUI.EndChangeCheck())
        {
            Preferences.FpsCapperEnable = fc_enable;
            Preferences.FpsCapperFrameRate = fc_frameRate;
            Preferences.VfxGraphZoomStep = vg_zoomStep;
        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        base.OnGUI(search);
    }

    [SettingsProvider]
    public static SettingsProvider GetSettingsProvider()
      => new PreferencesProvider();
}

} // namespace Klak.KlutterTools 
