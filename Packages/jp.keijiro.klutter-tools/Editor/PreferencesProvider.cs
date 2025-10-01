using UnityEditor;
using UnityEngine;

namespace KlutterTools {

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

    public static bool VfxGraphChangeFont
      { get => EditorPrefs.GetBool(Keys.VfxGraphChangeFont, false);
        set => EditorPrefs.SetBool(Keys.VfxGraphChangeFont, value); }

    public static int VfxGraphFontSize
      { get => EditorPrefs.GetInt(Keys.VfxGraphFontSize, 12);
        set => EditorPrefs.SetInt(Keys.VfxGraphFontSize, value); }

    // Preference keys
    static class Keys
    {
        public const string FpsCapperEnable
          = "KlutterTools.FpsCapper.Enable";

        public const string FpsCapperFrameRate
          = "KlutterTools.FpsCapper.FrameRate";

        public const string VfxGraphZoomStep
          = "KlutterTools.VfxGraph.ZoomStep";

        public const string VfxGraphChangeFont
          = "KlutterTools.VfxGraph.ChangeFont";

        public const string VfxGraphFontSize
          = "KlutterTools.VfxGraph.FontSize";
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
        var vg_changeFont = Preferences.VfxGraphChangeFont;
        var vg_fontSize = Preferences.VfxGraphFontSize;

        GUILayout.Label("FPS Capper", EditorStyles.boldLabel);
        fc_enable = EditorGUILayout.Toggle("Enable", fc_enable);
        using (new EditorGUI.DisabledScope(!fc_enable))
        {
            EditorGUI.indentLevel++;
            fc_frameRate = EditorGUILayout.IntField("Target Frame Rate", fc_frameRate);
            EditorGUI.indentLevel--;
        }

        GUILayout.Space(h);

        GUILayout.Label("VFX Graph", EditorStyles.boldLabel);
        vg_zoomStep = EditorGUILayout.FloatField("Zoom Step Scale", vg_zoomStep);

        vg_changeFont = EditorGUILayout.Toggle("Change Font", vg_changeFont);
        using (new EditorGUI.DisabledScope(!vg_changeFont))
        {
            EditorGUI.indentLevel++;
            vg_fontSize = EditorGUILayout.IntField("Font Size", vg_fontSize);
            EditorGUI.indentLevel--;
        }

        if (EditorGUI.EndChangeCheck())
        {
            Preferences.FpsCapperEnable = fc_enable;
            Preferences.FpsCapperFrameRate = fc_frameRate;
            Preferences.VfxGraphZoomStep = vg_zoomStep;
            Preferences.VfxGraphChangeFont = vg_changeFont;
            Preferences.VfxGraphFontSize = vg_fontSize;

            VfxGraphTextEditorModifier.ApplyToAllOpenWindows();
        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        base.OnGUI(search);
    }

    [SettingsProvider]
    public static SettingsProvider GetSettingsProvider()
      => new PreferencesProvider();
}

} // namespace KlutterTools
