using System;
using System.IO;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Networking;

namespace KlutterTools.Downloader {

[Serializable]
public sealed class FileEntry
{
    //// Editable attributes

    [field:SerializeField]
    public string SourceUrl { get; private set; }

    [field:SerializeField]
    public string Destination { get; private set; } = "Assets/StreamingAssets";

    //// Exposed UI properties

    [CreateProperty]
    public string Filename { get; private set; }

    [CreateProperty]
    public FileState CurrentState { get; private set; }

    //// Dynamic properties

    public string DestinationPath
      => Path.Combine(Destination, Filename);

    public string TemporaryPath
      => Path.Combine(Application.temporaryCachePath, Filename);

    //// Public methods

    // Validation method
    public void OnValidate()
    {
        if (Uri.TryCreate(SourceUrl, UriKind.Absolute, out var uri))
        {
            Filename = Path.GetFileName(uri.LocalPath);
            CurrentState = File.Exists(DestinationPath)
              ? FileState.Downloaded : FileState.Missing;
        }
        else
        {
            Filename = null;
            CurrentState = FileState.Missing;
        }
    }

    // Asynchronous file download method
    public async Awaitable<bool> DownloadAsync()
    {
        var destPath = DestinationPath;
        var tempPath = TemporaryPath;
        var success = false;

        using (var request = UnityWebRequest.Get(SourceUrl))
        {
            CurrentState = FileState.Downloading;
            request.downloadHandler = new DownloadHandlerFile(tempPath);
            await Awaitable.FromAsyncOperation(request.SendWebRequest());
            success = (request.result == UnityWebRequest.Result.Success);
            CurrentState = success ? FileState.Downloaded : FileState.Missing;
        }

        if (success)
            File.Move(tempPath, destPath);
        else
            Debug.LogError($"Failed to download test data file: {Filename}");

        return success;
    }
}

} // namespace KlutterTools.Downloader
