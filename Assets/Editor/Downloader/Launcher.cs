using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KlutterTools.Downloader {

[InitializeOnLoad]
static class Launcher
{
    public static Manifest GlobalManifest { get; private set; }

    static Launcher()
    {
        // Manifest asset search
        var guids = AssetDatabase.FindAssets("t:KlutterTools.Downloader.Manifest");
        if (guids.Length == 0) return;

        // Manifest asset load
        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        GlobalManifest = AssetDatabase.LoadAssetAtPath<Manifest>(path);

        // Session state check
        const string sessionKey = "KlutterTools.Downloader.Shown";
        //if (SessionState.GetBool(sessionKey, false)) return;
        SessionState.SetBool(sessionKey, true);

        // Open the missing entry window if there are any missing files.
        var missing = GlobalManifest.FileEntries.CollectMissing();
        if (missing.Any()) 
            EditorApplication.delayCall += MissingEntryWindow.ShowWindow;
    }
}

} // namespace KlutterTools.Downloader
