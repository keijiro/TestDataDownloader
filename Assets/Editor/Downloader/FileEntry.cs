using System;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Properties;

namespace KlutterTools.Downloader {

[Serializable]
public sealed class FileEntry
{
    public enum State { Missing, Downloading, Downloaded }

    [field:SerializeField]
    public string SourceUrl { get; private set; }

    [field:SerializeField]
    public string Destination { get; private set; } = "Assets/StreamingAssets";

    [CreateProperty]
    public string Filename { get; private set; }

    [CreateProperty]
    public State CurrentState { get; private set; }

    public string DestinationPath
      => Path.Combine(Destination, Filename);

    public string TemporaryPath
      => Path.Combine(Application.temporaryCachePath, Filename);

    public void OnValidate()
      => Filename = Uri.TryCreate(SourceUrl, UriKind.Absolute, out var uri)
           ? Path.GetFileName(uri.LocalPath) : null;

    // Asynchronous file download method
    public async Task<bool> DownloadAsync()
    {
        var url = SourceUrl;
        var destPath = DestinationPath;
        var tempPath = TemporaryPath;
        var success = false;

        using (var request = UnityWebRequest.Get(url))
        {
            request.downloadHandler = new DownloadHandlerFile(tempPath);
            await Awaitable.FromAsyncOperation(request.SendWebRequest());
            success = (request.result == UnityWebRequest.Result.Success);
        }

        if (success)
        {
            File.Move(tempPath, destPath);
        }
        else
        {
            Debug.LogError($"Failed to download test data file: {url}");
        }

        return success;
    }
}

} // namespace KlutterTools.Downloader
