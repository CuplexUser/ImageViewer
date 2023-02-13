using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using ImageViewer.Models;
using ImageViewer.Properties;

namespace ImageViewer.Services;

[UsedImplicitly]
public class UpdateService : ServiceBase
{
    private readonly Regex _downloadUrlRegex = new(@"^(URL = )(https://.*\.msi)");
    private readonly Regex _versionRegex = new(@"^(Version = )(\d\.\d\.\d\.\d)$");

    public async Task<bool> IsLatestVersion()
    {
        ApplicationVersion latestVersion = await GetLatestVersion();
        Version version = Assembly.GetExecutingAssembly().GetName().Version;
        if (version != null)
        {
            ApplicationVersion currentVersion = ApplicationVersion.Parse(version.ToString());

            return currentVersion.CompareTo(latestVersion) >= 0;
        }

        return false;
    }

    public async Task DownloadAndRunLatestVersionInstaller()
    {
        string path = await DownloadLatestVersion();
        try
        {
            Process.Start(path);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "DownloadAndRunLatestVersionInstaller");
        }
    }

    private async Task<ApplicationVersion> GetLatestVersion()
    {
        string url = Settings.Default.UpdateHistoryUrl;
        List<ApplicationVersion> versions;
        using (var client = new HttpClient())
        {
            Stream response = await client.GetStreamAsync(url);
            var streamReader = new StreamReader(response);
            versions = ParseFromUpdateTextFile(streamReader);
            versions.Sort();
        }

        return versions.Count > 0 ? versions.Last() : null;
    }

    private async Task<string> DownloadLatestVersion()
    {
        ApplicationVersion latestVersion = await GetLatestVersion();
        string tempDir = Path.GetTempPath();
        string url = latestVersion.DownloadUrl;
        if (string.IsNullOrEmpty(url)) throw new ArgumentException("Download Url can not be empty");

        string fileName = url.Split("/".ToCharArray()).Last();
        string downloadFilePath = Path.Combine(tempDir, fileName);

        if (File.Exists(downloadFilePath))
            File.Delete(downloadFilePath);

        using (var client = new HttpClient())
        {
            byte[] imageBytes = await client.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(downloadFilePath, imageBytes);
            //client.DownloadFile(url, downloadFilePath);
        }

        return downloadFilePath;
    }

    private List<ApplicationVersion> ParseFromUpdateTextFile(StreamReader stream)
    {
        var versions = new List<ApplicationVersion>();
        var downloadUrl = "";

        while (!stream.EndOfStream)
        {
            string line = stream.ReadLine();
            if (line != null && _downloadUrlRegex.IsMatch(line))
            {
                GroupCollection matches = _downloadUrlRegex.Match(line).Groups;
                if (matches.Count > 0) downloadUrl = matches[^1].Value;
            }
            else if (line != null && _versionRegex.IsMatch(line))
            {
                GroupCollection matches = _versionRegex.Match(line).Groups;
                if (matches.Count > 0) versions.Add(ApplicationVersion.Parse(matches[^1].Value, downloadUrl));
            }
        }

        return versions;
    }
}