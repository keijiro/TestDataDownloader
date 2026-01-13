using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KlutterTools.Downloader {

static class FileUtils
{
    public static IEnumerable<FileEntry>
      CollectMissing(this IEnumerable<FileEntry> entries)
        => entries.Where(e => !File.Exists(e.DestinationPath));
}

} // namespace KlutterTools.Downloader
