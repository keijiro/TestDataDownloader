using UnityEditor;
using UnityEngine;

namespace TestDataDownloader {

[InitializeOnLoad]
static class Launcher
{
    static Launcher()
      => EditorApplication.delayCall += OnDelayedSetup;

    static void OnDelayedSetup()
    {
        // Check session state.
        const string sessionKey = "TestDataDownloader.Shown";
        //if (SessionState.GetBool(sessionKey, false)) return;
        SessionState.SetBool(sessionKey, true);

        // Find dataset asset.
        var guids = AssetDatabase.FindAssets("t:TestDataDownloader.Dataset");
        if (guids.Length == 0) return;
        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        var dataset = AssetDatabase.LoadAssetAtPath<Dataset>(path);

        // Get missing file list.
        var urls = FileUtils.CheckMissingFiles(dataset.SourceUrls);
        if (urls.Length == 0) return;

        // Open up missing file list window.
        MissingFileListWindow.ShowWindow(urls);
    }
}

} // namespace TestDataDownloader
