using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace KlutterTools {

#if KLUTTER_TOOLS_HAS_VFXGRAPH

[InitializeOnLoad]
public static class VfxGraphTextEditorModifier
{
    #region Constructor and event handlers

    static VfxGraphTextEditorModifier()
    {
        // Subscribe to window focus change events
        EditorWindow.windowFocusChanged += OnWindowFocusChanged;

        // Double delayCall to wait for VFXTextEditor UI to be fully constructed after domain reload
        EditorApplication.delayCall += () => {
            EditorApplication.delayCall += () => { ApplyToAllOpenWindows(); };
        };
    }

    static void OnWindowFocusChanged()
      => EditorApplication.delayCall += () =>
           { CheckAndApplyCustomFont(EditorWindow.focusedWindow); };

    #endregion

    #region Public methods

    public static void ApplyToAllOpenWindows()
    {
        foreach (var window in Resources.FindObjectsOfTypeAll<EditorWindow>())
            CheckAndApplyCustomFont(window);
    }

    #endregion

    #region Font application logic

    static readonly string TextClassName
      = "unity-text-element--inner-input-field-component";

    static Font _font;

    static bool CheckWindowType(EditorWindow window)
        => window?.GetType()?.Name == "VFXTextEditor";

    static void ApplyCustomFont(EditorWindow window)
    {
        if (!Preferences.VfxGraphChangeFont) return;

        var root = window.rootVisualElement;
        var elements = root.Query(className: TextClassName).ToList();
        if (elements.Count == 0) return;

        if (_font == null)
            _font = Font.CreateDynamicFontFromOSFont
              (new[]{"Courier New"}, Preferences.VfxGraphFontSize);

        var unityFont = new StyleFont(_font);

        foreach (var element in elements)
        {
            element.style.unityFontDefinition = StyleKeyword.None;
            element.style.unityFont = unityFont;
            element.style.fontSize = Preferences.VfxGraphFontSize;
        }
    }

    static void CheckAndApplyCustomFont(EditorWindow window)
    {
        if (CheckWindowType(window)) ApplyCustomFont(window);
    }

    #endregion
}

#endif

} // namespace KlutterTools
