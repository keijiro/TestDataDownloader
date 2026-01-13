using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace KlutterTools.Downloader {

sealed class MissingEntryWindow : EditorWindow
{
    List<FileEntry> _entries;

    public static void ShowWindow(IEnumerable<FileEntry> missing)
    {
        var window = GetWindow<MissingEntryWindow>(true, "Missing Files");
        window.minSize = window.maxSize = new Vector2(400, 200);
        window._entries = missing.ToList();
    }

    void OnGUI()
    {
        // Close immediately if there are no missing entry.
        if (_entries.Count == 0)
        {
            Close();
            return;
        }

        // Message label
        EditorGUILayout.Space(12);
        EditorGUILayout.LabelField("Some test files are missing.");

        // Missing entry list
        EditorGUILayout.Space(8);
        DrawEntryList();
    }

    // HashSet to track active downloads
    readonly HashSet<string> _activeDownloads = new HashSet<string>();

    // Missing entry list along with download buttons
    void DrawEntryList()
    {
        foreach (var entry in _entries)
        {
            var isActive = _activeDownloads.Contains(entry.SourceUrl);
            var buttonLabel = isActive ? "Downloading..." : "Download";
            var buttonWidth = GUILayout.Width(120);

            EditorGUILayout.BeginHorizontal();

            // Filename label
            EditorGUILayout.LabelField(entry.Filename);

            // Download button
            EditorGUI.BeginDisabledGroup(isActive);
            if (GUILayout.Button(buttonLabel, buttonWidth)) DownloadAsync(entry);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
        }
    }

    // Asynchronous file download method
    async void DownloadAsync(FileEntry entry)
    {
        var url = entry.SourceUrl;
        if (!_activeDownloads.Add(url)) return;

        var destPath = entry.DestinationPath;
        var tempPath = entry.TemporaryPath;
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
            AssetDatabase.Refresh();
            _entries.Remove(entry);
        }
        else
        {
            Debug.LogError($"Failed to download test data file: {url}");
        }

        _activeDownloads.Remove(url);
        Repaint();
    }
}

} // namespace KlutterTools.Downloader
