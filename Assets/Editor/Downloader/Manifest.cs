using System;
using System.IO;
using UnityEngine;
using Unity.Properties;

namespace KlutterTools.Downloader {

[Serializable]
public sealed class FileEntry
{
    [field:SerializeField] public string SourceUrl { get; private set; }
    [field:SerializeField] public string Destination { get; private set; } = "Assets/StreamingAssets";

    [CreateProperty] public string Filename { get; private set; }

    public string DestinationPath => Path.Combine(Destination, Filename);
    public string TemporaryPath => Path.Combine(Application.temporaryCachePath, Filename);

    public void OnValidate()
      => Filename = Uri.TryCreate(SourceUrl, UriKind.Absolute, out var uri)
           ? Path.GetFileName(uri.LocalPath) : null;
}

[CreateAssetMenu(fileName = "Manifest", menuName = "Klutter Tools/Downloader/Manifest")]
public sealed class Manifest : ScriptableObject
{
    [field:SerializeField] public FileEntry[] FileEntries { get; private set; }

    void OnValidate()
    {
        if (FileEntries != null) foreach (var entry in FileEntries) entry?.OnValidate();
    }
}

} // namespace KlutterTools.Downloader
