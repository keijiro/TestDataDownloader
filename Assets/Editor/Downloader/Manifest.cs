using UnityEngine;

namespace KlutterTools.Downloader {

[CreateAssetMenu(fileName = "Manifest", menuName = "Klutter Tools/Downloader/Manifest")]
public sealed class Manifest : ScriptableObject
{
    [field:SerializeField]
    public FileEntry[] FileEntries { get; private set; }

    void OnValidate()
    {
        if (FileEntries == null) return;
        foreach (var entry in FileEntries) entry?.OnValidate();
    }
}

} // namespace KlutterTools.Downloader
