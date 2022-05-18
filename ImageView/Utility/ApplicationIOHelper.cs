using ImageViewer.Models.Import;
using JetBrains.Annotations;
using Serilog;
using System.Diagnostics;

namespace ImageViewer.Utility
{
    public static class ApplicationIOHelper
    {
        public static void EnumerateFiles(ref OutputDirectoryModel sourceFolder, string[] searchPattern, bool recursive = true)
        {
            try
            {
                sourceFolder.ImageList = new List<ImageRefModel>();
                var dirInfo = new DirectoryInfo(sourceFolder.FullName);

                var files = dirInfo
                    .GetFiles("*.*", SearchOption.TopDirectoryOnly)
                    .Where(file => searchPattern.Any(file.Extension.ToLower().Equals))
                    .ToList();

                files.Sort((info, fileInfo) => string.Compare(info.Name, fileInfo.Name, StringComparison.Ordinal));
                int index = 0;
                foreach (var file in files)
                {
                    var imgRef = new ImageRefModel
                    {
                        SortOrder = index++,
                        CompletePath = file.FullName,
                        FileSize = file.Length,
                        FileName = file.Name,
                        Directory = file.DirectoryName,
                        ImageType = file.Extension,
                        LastModified = file.LastWriteTime,
                        CreationTime = file.CreationTime,
                        ParentFolderId = sourceFolder.Id,
                    };
                    sourceFolder.ImageList.Add(imgRef);
                }

                if (sourceFolder.SubFolders != null && recursive)
                {
                    foreach (var sourceFolderModel in sourceFolder.SubFolders)
                    {
                        var outputDirectoryModel = sourceFolderModel;
                        outputDirectoryModel.ParentDirectory = sourceFolder;
                        EnumerateFiles(ref outputDirectoryModel, searchPattern);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "EnumerateFiles exception");
            }
        }

        public static bool OpenImageInDefaultAplication([NotNull] string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                    throw new ArgumentException("File does not exist", nameof(fileName));

                ProcessStartInfo psi = new ProcessStartInfo(fileName)
                {
                    UseShellExecute = true
                };
                Process.Start(psi);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "OpenImageInDefaultAplication: {Message}", ex.Message);
                return false;
            }
        }
    }
}