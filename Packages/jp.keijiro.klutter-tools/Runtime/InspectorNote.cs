using UnityEngine;

namespace KlutterTools {

[AddComponentMenu("Miscellaneous/Note")]
public sealed class InspectorNote : MonoBehaviour
{
#if UNITY_EDITOR

#pragma warning disable CS0414

    [SerializeField] string _noteText =
      "To edit the note, select \"Edit Note\" from the context menu.";

    [SerializeField] bool _showEditor = false;

    [SerializeField] Color _textColor = new Color32(210, 210, 210, 255);
    [SerializeField] Color _backgroundColor = new Color32(38, 38, 38, 255);

#pragma warning restore CS0414

    [ContextMenu("Edit Note")]
    void ShowNoteEditor() => _showEditor = true;

#endif
}

} // namespace KlutterTools
