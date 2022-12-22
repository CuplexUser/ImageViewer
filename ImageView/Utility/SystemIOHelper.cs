using System.Globalization;

namespace ImageViewer.Utility;

public static class SystemIOHelper
{
    public static long GetFileSize(string filename)
    {
        var fileInfo = new FileInfo(filename);
        return fileInfo.Length;
    }

    public static string FormatFileSizeToString(long byteCount, int decimales = 1)
    {
        string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
        if (byteCount == 0)
            return "0" + suf[0];
        long bytes = Math.Abs(byteCount);
        var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        double num = Math.Round(bytes / Math.Pow(1024, place), decimales);
        return (Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture) + " " + suf[place];
    }

    public static string GetFileExtention(string fileName)
    {
        if (fileName == null)
            return null;

        int index = fileName.LastIndexOf(".", StringComparison.Ordinal);
        return index != -1 ? fileName.Substring(index, fileName.Length - index) : null;
    }
}