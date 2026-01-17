using UnityEditor;

namespace KlutterTools.Downloader {

[InitializeOnLoad]
static class Launcher
{
    static Launcher()
    {
        // Session state check
        const string sessionKey = "KlutterTools.Downloader.Shown";
        if (SessionState.GetBool(sessionKey, false)) return;
        SessionState.SetBool(sessionKey, true);

        // Manifest existence check
        if (GlobalManifest.Instance == null) return;

        // All files downloaded check
        if (GlobalManifest.Instance.CheckAllDownloaded()) return;

        // Missing entry window open
        EditorApplication.delayCall += MissingEntryWindow.ShowWindow;
    }
}

} // namespace KlutterTools.Downloader
