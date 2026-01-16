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
        window.Show();
    }

    public void CreateGUI()
    {
        // New UI document from UXML
        var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
        var doc = uxml.CloneTree();

        // List view setup
        var list = doc.Q<ListView>("entry-list");
        list.bindItem = BindItem;
        list.itemsSource = GlobalManifest.Instance.FileEntries;

        rootVisualElement.Add(doc);
    }

    // List view item binding
    void BindItem(VisualElement element, int index)
    {
        var button = element.Q<Button>("download-button");
        var entry = GlobalManifest.Instance.FileEntries[index];

        // Enable button only for missing files.
        button.enabledSelf = (entry.CurrentState == FileState.Missing);

        // Download button handler
        button.clicked += async () =>
        {
            button.enabledSelf = false;
            if (await entry.DownloadAsync())
                AssetDatabase.Refresh();
            else
                button.enabledSelf = true;
            Repaint();
        };
    }
}

} // namespace KlutterTools.Downloader
