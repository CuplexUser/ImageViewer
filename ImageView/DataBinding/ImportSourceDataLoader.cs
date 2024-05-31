using ImageViewer.Models.Import;
using System.Security;

namespace ImageViewer.DataBinding;

public class ImportSourceDataLoader
{
    public static List<SourceFolderModel> GetSubfolders(SourceCollectionBase parent)
    {
        var folderNodeLIst = new List<SourceFolderModel>();
        DirectoryInfo dirInfo = null;
        DirectoryInfo[] subdirs = null;
        try
        {
            dirInfo = new DirectoryInfo(parent.GetFolderPath());
            subdirs = dirInfo.GetDirectories();
        }
        catch (SecurityException exception)
        {
            Log.Information("Unable to list the following directory: {FolderPath}", exception, parent.GetFolderPath());
        }
        catch (Exception exception)
        {
            Log.Information("Unable to list the following directory: {FolderPath}", exception, parent.GetFolderPath());
        }

        if (dirInfo == null || subdirs == null)
        {
            return new List<SourceFolderModel>();
        }

        //var 

        int index = 0;
        foreach (var subdir in subdirs)
        {
            if (((subdir.Attributes & FileAttributes.System) != 0) | ((subdir.Attributes & FileAttributes.Hidden) != 0))
            {
                continue;
            }

            folderNodeLIst.Add(new SourceFolderModel(subdir.FullName)
            {
                Name = subdir.Name,
                ParentFolderId = parent.Id,
                ImageList = new List<ImageRefModel>(),
                SortOrder = index++
            });
        }

        return folderNodeLIst;
    }
}