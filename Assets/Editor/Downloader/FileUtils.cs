using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace KlutterTools.Downloader {

static class FileUtils
{
    public static string UrlToFilename(string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            return Path.GetFileName(uri.LocalPath);
        return null;
    }

    public static string GetDestinationPath(FileEntry entry)
      => Path.Combine(entry.Destination, UrlToFilename(entry.SourceUrl));

    public static string GetTemporaryPath(FileEntry entry)
      => Path.Combine(Application.temporaryCachePath, UrlToFilename(entry.SourceUrl));

    public static IEnumerable<FileEntry>
      CollectMissing(this IEnumerable<FileEntry> entries)
        => entries.Where(e => !File.Exists(GetDestinationPath(e)));
}

} // namespace KlutterTools.Downloader
