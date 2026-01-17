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

        // Downloader window open
        EditorApplication.delayCall += DownloaderWindow.ShowWindow;
    }
}

} // namespace KlutterTools.Downloader
