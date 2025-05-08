using UnityEngine;

namespace KlutterTools {

[AddComponentMenu("Miscellaneous/Note")]
public sealed class InspectorNote : MonoBehaviour
{
#if UNITY_EDITOR

#pragma warning disable CS0414

    [SerializeField] string _noteText =
      "To edit the note, select \"Toggle Note Editor\" from the inspector context menu.";

    [SerializeField] bool _showEditor = false;

    [SerializeField] Color _backgroundColor = new Color(0.15f, 0.15f, 0.15f, 1);

#pragma warning restore CS0414

    [ContextMenu("Toggle Note Editor")]
    void ToggleNoteEditor() => _showEditor = !_showEditor;

#endif
}

} // namespace KlutterTools
