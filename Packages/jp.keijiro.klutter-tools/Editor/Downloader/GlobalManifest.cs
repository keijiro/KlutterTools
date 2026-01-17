using UnityEditor;

namespace KlutterTools.Downloader {

public static class GlobalManifest
{
    public static Manifest Instance { get; private set; }

    static GlobalManifest()
    {
        // Search
        var guids = AssetDatabase.FindAssets("t:KlutterTools.Downloader.Manifest");
        if (guids.Length == 0) return;

        // Load
        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        Instance = AssetDatabase.LoadAssetAtPath<Manifest>(path);
        Instance.Validate();
    }
}

} // namespace KlutterTools.Downloader
