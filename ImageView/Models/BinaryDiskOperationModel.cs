namespace ImageViewer.Models
{
    public class BinaryDiskOperationModel
    {
        public BinaryDiskOperationModel(AccessMode mode, long position, int length)
        {
            Mode = mode;
            Position = position;
            Length = length;
        }

        public AccessMode Mode { get;  }

        public long Position { get;  }

        public int Length { get; }

        public async Task<byte[]> ReadDataAsync()
        {

        }

        public async Task WriteDataAsync(byte[] dataBytes)
        {

        }

        public enum AccessMode
        {
            Read,
            Write
        }
    }
}