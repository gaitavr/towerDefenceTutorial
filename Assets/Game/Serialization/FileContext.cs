using System.IO;
using UnityEngine;

public class FileContext
{
    public string CreationTime { get; private set; }
    public string FileName { get; private set; }

    private const string DateFormat = "yyyy/MM/dd HH:mm:ss";
    
    public FileContext(FileInfo file)
    {
        CreationTime = file.CreationTimeUtc.ToString(DateFormat);
        FileName = Path.GetFileNameWithoutExtension(file.Name);
    }
}