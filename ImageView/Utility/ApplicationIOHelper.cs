using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ImageViewer.Models.Import;
using JetBrains.Annotations;
using Serilog;

namespace ImageViewer.Utility
{
    public static class ApplicationIOHelper
    {
        public static void EnumerateFiles(ListViewSourceModel sourceFolder, string[] searchPattern, bool recursive = true)
        {
            try
            {
                sourceFolder.ImageList = new List<ImageRefModel>();
                var dirInfo = new DirectoryInfo(sourceFolder.FullName);

                var files = dirInfo
                    .GetFiles("*.*", SearchOption.TopDirectoryOnly)
                    .Where(file => searchPattern.Any(file.Extension.ToLower().Equals))
                    .ToList();

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

                if (sourceFolder.Folders != null && recursive)
                {
                    foreach (var sourceFolderModel in sourceFolder.Folders)
                    {
                        EnumerateFiles(sourceFolderModel, searchPattern);
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
                Log.Error(ex, "OpenImageInDefaultAplication: {Message}",ex.Message);
                return false;
            }
        }
    }
}