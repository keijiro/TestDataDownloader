using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace KlutterTools.Downloader {

sealed class MissingEntryWindow : EditorWindow
{
    static readonly string UxmlPath = "Assets/Editor/Downloader/MissingEntryWindow.uxml";

    public static void ShowWindow()
    {
        var window = GetWindow<MissingEntryWindow>(true, "Missing Files");
        window.minSize = new Vector2(400, 200);
    }

    public void CreateGUI()
    {
        var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
        var doc = uxml.CloneTree();

        var list = doc.Q<ListView>("entry-list");
        var entries = Launcher.GlobalManifest.FileEntries;
        list.itemsSource = entries;
        list.bindItem = (element, i) =>
        {
            var entry = entries[i];
            var button = element.Q<Button>("download-button");
            if (button.userData is System.Action prev)
                button.clicked -= prev;

            System.Action handler = async () =>
            {
                Debug.Log($"Download clicked: {entry.Filename}");
                button.enabledSelf = false;
                if (await entry.DownloadAsync()) AssetDatabase.Refresh();
                Repaint();
            };

            button.userData = handler;
            button.clicked += handler;
        };

        rootVisualElement.Add(doc);
    }
}

} // namespace KlutterTools.Downloader
