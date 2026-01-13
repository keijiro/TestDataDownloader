using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TestDataDownloader {

[CreateAssetMenu(fileName = "Dataset", menuName = "Klak/Hap/Test Dataset")]
public sealed class Dataset : ScriptableObject
{
    [field:SerializeField] public string[] SourceUrls { get; private set; }
}

} // namespace TestDataDownloader
