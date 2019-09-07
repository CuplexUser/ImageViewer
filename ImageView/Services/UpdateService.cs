using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageViewer.Models;
using JetBrains.Annotations;

namespace ImageViewer.Services
{
    [UsedImplicitly]
    public class UpdateService : ServiceBase
    {
        private readonly Regex _versionRegex = new Regex(@"^(Version = )(\d\.\d\.\d\.\d)$");
        private readonly Regex _downloadUrlRegex = new Regex(@"^(URL = )(https://.*\.msi)");

        public async Task<bool> IsLatestVersion()
        {
            var latestVersion = await GetLatestVersion();
            var currentVersion = ApplicationVersion.Parse(Assembly.GetExecutingAssembly().GetName().Version.ToString());

            return currentVersion.CompareTo(latestVersion) >= 0;
        }

        public async Task DownloadAndRunLatestVersionInstaller()
        {
            string path = await DownloadLatestVersion();
            Process.Start(path);
        }

        private async Task<ApplicationVersion> GetLatestVersion()
        {
            string url = Properties.Settings.Default.UpdateHistoryUrl;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            var response = await req.GetResponseAsync();
            var streamReader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
            var versions = ParseFromUpdateTextFile(streamReader);
            versions.Sort();

            return versions.Count > 0 ? versions.Last() : null;
        }

        private async Task<string> DownloadLatestVersion()
        {
            var latestVersion = await GetLatestVersion();
            var tempDir = Path.GetTempPath();
            string url = latestVersion.DownloadUrl;
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("Download Url can not be empty");
            }

            string fileName = url.Split("/".ToCharArray()).Last();
            string downloadFilePath = Path.Combine(tempDir, fileName);

            if (File.Exists(downloadFilePath))
                File.Delete(downloadFilePath);

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, downloadFilePath);
            }

            return downloadFilePath;
        }

        private List<ApplicationVersion> ParseFromUpdateTextFile(StreamReader stream)
        {
            var versions = new List<ApplicationVersion>();
            string downloadUrl = "";

            while (!stream.EndOfStream)
            {
                string line = stream.ReadLine();
                if (line != null && _downloadUrlRegex.IsMatch(line))
                {
                    var matches = _downloadUrlRegex.Match(line).Groups;
                    if (matches.Count > 0)
                    {
                        downloadUrl = matches[matches.Count - 1].Value;
                    }
                }
                else if (line != null && _versionRegex.IsMatch(line))
                {
                    var matches = _versionRegex.Match(line).Groups;
                    if (matches.Count > 0)
                    {
                        versions.Add(ApplicationVersion.Parse(matches[matches.Count - 1].Value, downloadUrl));
                    }
                }
            }

            return versions;
        }
    }
}
