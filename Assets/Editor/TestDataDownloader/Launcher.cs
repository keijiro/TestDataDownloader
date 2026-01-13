using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KlutterTools.Downloader {
[InitializeOnLoad]
static class Launcher
{
    static Launcher()
      => EditorApplication.delayCall += OnDelayedSetup;

    static void OnDelayedSetup()
    {
        // Check session state.
        const string sessionKey = "KlutterTools.Downloader.Shown";
        //if (SessionState.GetBool(sessionKey, false)) return;
        SessionState.SetBool(sessionKey, true);

        // Find dataset asset.
        var guids = AssetDatabase.FindAssets("t:KlutterTools.Downloader.Manifest");
        if (guids.Length == 0) return;
        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        var manifest = AssetDatabase.LoadAssetAtPath<Manifest>(path);

        // Get missing file list.
        var missing = manifest.FileEntries.CollectMissing();
        if (!missing.Any()) return;

        // Open up missing file list window.
        MissingEntryWindow.ShowWindow(missing);
    }
}

} // namespace KlutterTools.Downloader
