namespace ImageViewer.Providers;

public class BlobStorageProvider : ProviderBase, IDisposable
{
    private readonly string _blobStorageFilename;
    private FileStream _blobDataFileStream = null;
    private const int FileHeaderOffset = 32;
    private const string BlobStorageFileName = "thumbnailStorage.bin";
    private readonly ReaderWriterLockSlim _readerWriterLock;

    private int filePosition = 0;

    private static readonly byte[] HeaderBytes =
    {
        0x33, 0xB2, 0xC1, 0xDF, 0x23, 0xA2, 0xC9, 0x66, 0x73, 0xA6, 0x85, 0x8F, 0xE6, 0xA1, 0x06, 0x2E, 0xE8, 0xA9, 0x39, 0x76, 0xFB, 0x83, 0xE1, 0xF3, 0x2B, 0xF6, 0x19, 0x1D, 0xCC, 0x0C, 0xE1, 0xF
    };

    public bool StorageFileIsOpen => _blobDataFileStream != null && _blobDataFileStream.CanRead && _blobDataFileStream.CanWrite;

    public BlobStorageProvider()
    {
        _blobStorageFilename = Path.Join(GlobalSettings.Instance.GetUserDataDirectoryPath(), BlobStorageFileName);
        _readerWriterLock = new ReaderWriterLockSlim();
    }

    public bool OpenStorageFile()
    {
        try
        {
            if (StorageFileIsOpen)
                return false;


            if (!File.Exists(_blobStorageFilename))
            {
                CreateNewBlobStorageFile();
            }

            _blobDataFileStream = File.OpenWrite(_blobStorageFilename);

            //Verify header
            _blobDataFileStream.Position = 0;
            byte[] buffer = new byte[HeaderBytes.Length];
            int length = _blobDataFileStream.Read(buffer);

            if (length != buffer.Length)
                return false;

            bool isMatch = true;
            for (int i = 0; i < length; i++)
            {
                isMatch &= HeaderBytes[i] == buffer[i];
            }

            return isMatch;
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Error when opening file: {filename}", _blobStorageFilename);
        }

        return false;
    }

    public async Task<byte[]> ReadBlobDataAsync(int position, int length)
    {
        byte[] buffer = new byte[length];
        int bytesRead = 0;

        _readerWriterLock.TryEnterReadLock(2000);
        try
        {
            if (!_blobDataFileStream.CanRead)
                _blobDataFileStream = File.OpenWrite(BlobStorageFileName);
            _blobDataFileStream.Position = position;

            bytesRead = await _blobDataFileStream.ReadAsync(buffer, FileHeaderOffset + position, length);
        }
        finally
        {
            _readerWriterLock.ExitReadLock();
        }

        if (bytesRead != buffer.Length)
        {
            return null;
        }
        return buffer;
    }

    public async Task<int> WriteBlobDataAsync(byte[] data)
    {
        if (_readerWriterLock.TryEnterWriteLock(1000))
        {
            int currentFilePosition = -1;
            try
            {
                if (!StorageFileIsOpen)
                    _blobDataFileStream = File.OpenWrite(_blobStorageFilename);

                currentFilePosition = Convert.ToInt32(_blobDataFileStream.Length);
                _blobDataFileStream.Lock(filePosition, data.Length);
                await _blobDataFileStream.WriteAsync(data, filePosition, data.Length, new CancellationToken(false));
                _blobDataFileStream.Unlock(filePosition, data.Length);
            }
            finally
            {
                _readerWriterLock.ExitWriteLock();
            }



            return currentFilePosition;
        }

        return -1;
    }


    private void CreateNewBlobStorageFile()
    {
        if (_readerWriterLock.TryEnterWriteLock(5000))
        {
            try
            {
                if (File.Exists(_blobStorageFilename))
                    File.Delete(_blobStorageFilename);

                _blobDataFileStream = File.OpenWrite(_blobStorageFilename);
                _blobDataFileStream.Position = 0;
                _blobDataFileStream.Write(HeaderBytes);
                _blobDataFileStream.Flush();
            }
            finally
            {
                //_blobDataFileStream.Close();
                _readerWriterLock.ExitWriteLock();
            }
        }
    }

    public void Dispose()
    {
        if (_blobDataFileStream != null && _blobDataFileStream.CanWrite)
        {
            _blobDataFileStream.Flush(true);
            _blobDataFileStream.Close();
            _blobDataFileStream.Dispose();
            _blobDataFileStream = null;
        }
    }
}