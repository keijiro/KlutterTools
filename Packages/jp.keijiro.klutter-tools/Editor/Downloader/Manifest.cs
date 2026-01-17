using System.Linq;
using UnityEngine;

namespace KlutterTools.Downloader {

[CreateAssetMenu(fileName = "Manifest", menuName = "Klutter Tools/Downloader/Manifest")]
public sealed class Manifest : ScriptableObject
{
    // Editable attributes

    [field:SerializeField]
    public string Caption { get; private set; } = "Some files are missing:";

    [field:SerializeField]
    public FileEntry[] FileEntries { get; private set; }

    // Public methods

    public bool CheckAllDownloaded()
        => FileEntries.All(e => e.CurrentState == FileState.Downloaded);

    public void Validate() => OnValidate();

    // ScriptableObject implementation

    void OnValidate()
    {
        if (FileEntries == null) return;
        foreach (var entry in FileEntries) entry.OnValidate();
    }
}

} // namespace KlutterTools.Downloader
