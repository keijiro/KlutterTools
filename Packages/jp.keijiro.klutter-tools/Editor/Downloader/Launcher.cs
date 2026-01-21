using UnityEditor;

namespace KlutterTools.Downloader {

[InitializeOnLoad]
static class Launcher
{
    static Launcher()
      => EditorApplication.delayCall += CheckAndShow;

    static void CheckAndShow()
    {
        // Session state check
        const string sessionKey = "KlutterTools.Downloader.Shown";
        if (SessionState.GetBool(sessionKey, false)) return;

        // Manifest existence check
        if (GlobalManifest.Instance == null) return;

        // All files downloaded check
        if (GlobalManifest.Instance.CheckAllDownloaded()) return;

        // Downloader window open
        DownloaderWindow.ShowWindow();
        SessionState.SetBool(sessionKey, true);
    }
}

} // namespace KlutterTools.Downloader
