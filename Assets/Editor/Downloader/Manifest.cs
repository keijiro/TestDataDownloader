using System;
using System.IO;
using UnityEngine;

namespace KlutterTools.Downloader {

[Serializable]
public sealed class FileEntry
{
    [field:SerializeField] public string SourceUrl { get; private set; }
    [field:SerializeField] public string Destination { get; private set; } = "StreamingAssets";

    public string Filename
      => Uri.TryCreate(SourceUrl, UriKind.Absolute, out var uri)
           ? Path.GetFileName(uri.LocalPath) : null;

    public string DestinationPath
      => Path.Combine(Destination, Filename);

    public string TemporaryPath
      => Path.Combine(Application.temporaryCachePath, Filename);
}

[CreateAssetMenu(fileName = "Manifest", menuName = "Klutter Tools/Downloader/Manifest")]
public sealed class Manifest : ScriptableObject
{
    [field:SerializeField] public FileEntry[] FileEntries { get; private set; }
}

} // namespace KlutterTools.Downloader
