using System;
using UnityEngine;

namespace KlutterTools.Downloader {

[Serializable]
public sealed class FileEntry
{
    [field:SerializeField] public string SourceUrl { get; private set; }
    [field:SerializeField] public string Destination { get; private set; } = "StreamingAssets";
}

[CreateAssetMenu(fileName = "Manifest", menuName = "Klutter Tools/Downloader/Manifest")]
public sealed class Manifest : ScriptableObject
{
    [field:SerializeField] public FileEntry[] FileEntries { get; private set; }
}

} // namespace KlutterTools.Downloader
