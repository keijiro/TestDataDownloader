using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TestDataDownloader {

static class FileUtils
{
    public static string UrlToFilename(string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            return Path.GetFileName(uri.LocalPath);
        return null;
    }

    public static string GetDestinationPath(string filename)
      => Path.Combine(Application.streamingAssetsPath, filename);

    public static string GetTemporaryPath(string filename)
      => Path.Combine(Application.temporaryCachePath, filename);

    public static string[] CheckMissingFiles(string[] urls)
    {
        var missingList = new List<string>();
        foreach (var url in urls)
        {
            var filename = UrlToFilename(url);
            if (filename == null) continue;
            var destPath = GetDestinationPath(filename);
            if (!File.Exists(destPath)) missingList.Add(url);
        }
        return missingList.ToArray();
    }
}

} // namespace TestDataDownloader
