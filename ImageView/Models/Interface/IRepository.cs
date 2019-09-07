using System.Drawing;

namespace ImageViewer.Models.Interface
{
    public interface IRepository
    {
        bool IsLocked { get; }

        Image ReadImage(FileEntry fileEntry);

        FileEntry WriteImage(Image img);

        void SaveToDisk();

        void CloseStream();

        bool LockDatabase();

        void UnlockDatabase();
    }
}