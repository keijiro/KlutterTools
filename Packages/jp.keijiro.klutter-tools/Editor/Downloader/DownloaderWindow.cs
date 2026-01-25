using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace KlutterTools.Downloader {

sealed class DownloaderWindow : EditorWindow
{
    static readonly string UxmlPath =
      "Packages/jp.keijiro.klutter-tools/Editor/Downloader/DownloaderWindow.uxml";

    public static void ShowWindow()
    {
        var window = GetWindow<DownloaderWindow>(true, "Downloader");
        window.minSize = new Vector2(400, 200);
        window.Show();
    }

    public void CreateGUI()
    {
        // New UI document from UXML
        var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
        var doc = uxml.CloneTree();

        // Caption text
        var caption = doc.Q<Label>("caption-label");
        caption.text = GlobalManifest.Instance.Caption;

        // List view setup
        var list = doc.Q<ListView>("entry-list");
        list.bindItem = BindItem;
        list.itemsSource = GlobalManifest.Instance.FileEntries;

        // Download-all button setup
        var button = doc.Q<Button>("download-all-button");
        button.clicked += () => DownloadAllAsync(list, button).Forget();

        rootVisualElement.Add(doc);
    }

    async Awaitable DownloadAllAsync(ListView list, Button button)
    {
        button.enabledSelf = false;
        button.text = "Downloading...";
        try
        {
            foreach (var entry in GlobalManifest.Instance.FileEntries)
            {
                if (entry.CurrentState != FileState.Missing) continue;
                var download = entry.DownloadAsync();
                list.RefreshItems(); // To disable download button
                await download;
            }
            list.RefreshItems();
            AssetDatabase.Refresh();
        }
        finally
        {
            button.text = "Done";
        }
    }

    // List view item binding
    void BindItem(VisualElement element, int index)
    {
        var button = element.Q<Button>("download-button");
        var entry = GlobalManifest.Instance.FileEntries[index];

        // Enable button only for missing files.
        button.enabledSelf = (entry.CurrentState == FileState.Missing);

        // Remove previous handler if exists.
        if (button.userData is Action prev) button.clicked -= prev;

        // Register new handler.
        Action action = async () =>
        {
            button.enabledSelf = false;
            if (await entry.DownloadAsync())
                AssetDatabase.Refresh();
            else
                button.enabledSelf = true;
            Repaint();
        };
        button.clicked += action;
        button.userData = action;
    }
}

} // namespace KlutterTools.Downloader
