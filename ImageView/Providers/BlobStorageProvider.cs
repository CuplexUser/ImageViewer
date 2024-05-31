using GeneralToolkitLib.Hashing;


namespace ImageViewer.Providers;
[UsedImplicitly]
public class BlobStorageProvider : ProviderBase, IDisposable, IEqualityComparer<BlobStorageProvider>
{
    private const string BlobStorageFileName = "thumbnailStorage.bin";

    private static readonly byte[] HeaderBytes =
    [
        0xb5, 0xcb, 0x1e, 0x2b, 0xe6, 0x54, 0x15, 0x57, 0x90, 0x11, 0xe6, 0x20, 0xbc, 0x5a, 0x46, 0x7d,
        0x74, 0x86, 0x3f, 0x7d, 0x6c, 0x3d, 0x25, 0x57, 0xb0, 0xc4, 0xda, 0xa7, 0x89, 0x6b, 0x3e, 0xbe,
        0xbc, 0x35, 0xc8, 0xe6, 0xa4, 0xe2, 0xa8, 0x3c, 0x9d, 0xc9, 0xea, 0xd7, 0xcc, 0xb8, 0xc0, 0x8e,
        0x8f, 0x28, 0x8c, 0xea, 0xa9, 0xea, 0x5a, 0x20, 0x26, 0x17, 0xd6, 0x20, 0x9b, 0xdb, 0xa9, 0xa9,
        0xa1, 0x5c, 0x38, 0x0f, 0xe3, 0xe6, 0x8f, 0x67, 0x44, 0x3d, 0xfc, 0x9a, 0x0e, 0x0f, 0x3f, 0x21,
        0xdb, 0xb1, 0x39, 0x15, 0xf7, 0xca, 0x0d, 0x06, 0x9e, 0xbb, 0x16, 0x45, 0x61, 0xf7, 0x0f, 0x29,
        0x5f, 0x88, 0x5a, 0x1a, 0x64, 0x11, 0x02, 0x12, 0xff, 0xb9, 0xc7, 0xf6, 0x09, 0x22, 0x94, 0xfa,
        0x57, 0x7a, 0xca, 0xe2, 0x74, 0xe2, 0x28, 0x42, 0x84, 0x96, 0x8a, 0xd4, 0x6e, 0xa3, 0xae, 0x19,
        0xd4, 0x3e, 0x40, 0x2d, 0xd4, 0x68, 0x0a, 0xb7, 0x0e, 0x72, 0x94, 0xf5, 0x6a, 0x7a, 0xd9, 0xef,
        0x9c, 0xb6, 0xac, 0x8f, 0x00, 0xac, 0x1c, 0x32, 0xba, 0x41, 0xa5, 0xac, 0xcb, 0x27, 0x2b, 0x5d,
        0x90, 0x15, 0x21, 0x3f, 0xdd, 0x05, 0xd6, 0xd3, 0x05, 0x6c, 0x04, 0xbe, 0x36, 0x95, 0xc4, 0x2d,
        0x1b, 0xb1, 0xef, 0x98, 0x24, 0x69, 0xdc, 0x45, 0xdb, 0x29, 0x29, 0xfd, 0x90, 0xf9, 0x00, 0x11,
        0xeb, 0x85, 0xf7, 0x1f, 0xf0, 0xf3, 0xfc, 0x9c, 0x27, 0x14, 0x1c, 0x3b, 0xaa, 0x53, 0x2d, 0x25,
        0xe9, 0x4e, 0xe5, 0xdc, 0x2d, 0x7e, 0x3e, 0xaa, 0x76, 0xbd, 0x07, 0xa8, 0xeb, 0xd0, 0x51, 0x0c,
        0x2b, 0x07, 0xed, 0x9d, 0xca, 0xaf, 0xe6, 0xbe, 0xd6, 0xee, 0x50, 0xbd, 0x5d, 0x1a, 0x68, 0x44,
        0x97, 0xef, 0x75, 0xf5, 0x46, 0x11, 0x91, 0x2a, 0x8d, 0xfe, 0x67, 0x13, 0x63, 0x22, 0x83, 0xce
    ];

    private readonly string _blobStorageFilename;
    private readonly ReaderWriterLock _readerWriterLock;

    public readonly string InstanceId;

    private readonly object LockObject = new();
    private FileStream _blobDataFileStream;

    private long filePosition;
    private int WriteCount;

    public BlobStorageProvider()
    {
        _blobStorageFilename = Path.Join(GlobalSettings.Instance.GetUserDataDirectoryPath(), BlobStorageFileName);
        InstanceId = SHA256.GetSHA256HashAsHexString(_blobStorageFilename);
        _readerWriterLock = new ReaderWriterLock();
    }

    // Use a custom file name
    public BlobStorageProvider(string fileName)
    {
        _blobStorageFilename = Path.Join(GlobalSettings.Instance.GetUserDataDirectoryPath(), fileName);
        InstanceId = SHA256.GetSHA256HashAsHexString(_blobStorageFilename);
        _readerWriterLock = new ReaderWriterLock();
    }

    public bool OpenStorageFile()
    {
        try
        {
            if (StorageFileIsOpen)
            {
                return false;
            }

            if (!File.Exists(_blobStorageFilename))
            {
                if (!CreateNewBlobStorageFile())
                {
                    return false;
                }
            }

            _blobDataFileStream ??= File.Open(_blobStorageFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

            if (StorageFileId == null)
            {
                CreateStorageFileId();
            }

            //Verify header
            _blobDataFileStream.Position = 0;
            byte[] buffer = new byte[HeaderBytes.Length];
            int length = _blobDataFileStream.Read(buffer);

            if (length != buffer.Length)
            {
                return false;
            }

            filePosition = HeaderBytes.Length;
            _blobDataFileStream.Position = filePosition;

            if (buffer.SequenceEqual(HeaderBytes))
            {
                return true;
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Error when opening file: {filename}", _blobStorageFilename);
            if (_blobDataFileStream != null)
            {
                _blobDataFileStream.Close();
                _blobDataFileStream.Dispose();
                _blobDataFileStream = null;
            }

            return false;
        }

        return false;
    }

    private void CreateStorageFileId()
    {
        if (StorageFileIsOpen)
        {
            int length = 262144; //256 kb
            if (length > _blobDataFileStream.Length)
            {
                length = Convert.ToInt32(_blobDataFileStream.Length);
            }

            if (length < HeaderBytes.Length)
            {
                throw new InvalidOperationException($"data file size is less then the initial header bytes: actual size is: {_blobDataFileStream.Length}");
            }

            _blobDataFileStream.Position = 0;

            byte[] buffer = new byte[length];
            _blobDataFileStream.ReadAtLeast(buffer, length);

            StorageFileId = SHA256.GetSHA256HashAsByteArray(buffer);
        }
    }

    public async Task<byte[]> ReadBlobDataAsync(long position, int length)
    {
        if (length > _blobDataFileStream.Length)
        {
            throw new InvalidOperationException($"The length requested is larger then the file size, {_blobDataFileStream.Length}");
        }

        if (position < HeaderBytes.Length)
        {
            throw new InvalidOperationException($"data file size is less then the initial header bytes: actual size is: {_blobDataFileStream.Length}");
        }

        byte[] buffer = new byte[length];

        try
        {
            _readerWriterLock.AcquireReaderLock(-1);

            _blobDataFileStream.Position = position;

            // ReadAsync gives terrible performance and the file stream is thread safe so using readerWriterLock
            int bytesRead = await _blobDataFileStream.ReadAsync(buffer, 0, length);

            return bytesRead == length ? buffer : null;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "ReadBlobDataAsync failed");
            return null;
        }
        finally
        {
            _readerWriterLock.ReleaseReaderLock();

        }
    }

    public void WriteBlobData(byte[] data)
    {
        _readerWriterLock.AcquireWriterLock(0);
        if (!StorageFileIsOpen)
        {
            lock (LockObject)
            {
                OpenStorageFile();
            }
        }

        _blobDataFileStream.Position = _blobDataFileStream.Length;
        _blobDataFileStream.Write(data);

        _readerWriterLock.ReleaseLock();
    }



    // Task in order to make it awaitable
    public MemoryStream GetBlobData()
    {

        if (!StorageFileIsOpen)
        {
            lock (LockObject)
            {
                OpenStorageFile();
            }
        }

        try
        {
            _readerWriterLock.AcquireWriterLock(-1);

            var ms = new MemoryStream();
            _blobDataFileStream.Position = HeaderBytes.Length;
            _blobDataFileStream.CopyTo(ms);

            return ms;

        }
        catch (LockRecursionException)
        {
            Log.Error("GetBlobDataAsync error, file is being written");
        }
        finally
        {
            _readerWriterLock.ReleaseLock();
        }

        return null;
    }

    private bool CreateNewBlobStorageFile()
    {
        _readerWriterLock.AcquireWriterLock(-1);

        try
        {
            Interlocked.Increment(ref WriteCount);

            if (WriteCount != 1)
                throw new InvalidOperationException("File is being written");

            _blobDataFileStream = File.Open(_blobStorageFilename, FileMode.CreateNew);
            _blobDataFileStream.Position = 0;
            _blobDataFileStream.Write(HeaderBytes);
            _blobDataFileStream.Flush();
            filePosition = _blobDataFileStream.Position;

            return true;
        }
        finally
        {
            Interlocked.Decrement(ref WriteCount);
            _readerWriterLock.ReleaseLock();
        }
    }

    public long GetFileSize()
    {
        return StorageFileIsOpen ? _blobDataFileStream.Length : 0;
    }

    public async Task<bool> SaveFileToDiskAsync()
    {
        if (StorageFileIsOpen)
        {
            try
            {
                await _blobDataFileStream.FlushAsync();
                return true;
            }
            catch (Exception exception)
            {
                Log.Error(exception, "SaveFileToDiskAsync Exception");
            }
        }

        return false;
    }

    public bool ClearStorage()
    {
        try
        {
            _readerWriterLock.AcquireWriterLock(-1);

            if (_blobDataFileStream is { Length: > 0 })
            {
                _blobDataFileStream.Flush(true);
                _blobDataFileStream.Close();
                _blobDataFileStream = null;
            }

            if (File.Exists(_blobStorageFilename))
            {
                File.Delete(_blobStorageFilename);
            }

            CreateNewBlobStorageFile();

            return true;
        }
        finally
        {
            _readerWriterLock.ReleaseLock();
        }

        return true;
    }

    #region Public Class Properties

    public byte[] StorageFileId { get; private set; }

    public bool StorageFileIsOpen => _blobDataFileStream is { CanWrite: true };

    #endregion


    #region Implemented-Interfaces

    public void Dispose()
    {
        if (_blobDataFileStream is { CanWrite: true })
        {
            _blobDataFileStream.Flush(true);
            _blobDataFileStream.Close();
            _blobDataFileStream.Dispose();
            _blobDataFileStream = null;
        }
    }

    // Same file open equals true
    public bool Equals(BlobStorageProvider x, BlobStorageProvider y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (ReferenceEquals(x, null))
        {
            return false;
        }

        if (ReferenceEquals(y, null))
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return x.InstanceId == y.InstanceId;
    }

    public int GetHashCode(BlobStorageProvider obj)
    {
        return obj.InstanceId != null ? obj.InstanceId.GetHashCode() : 0;
    }

    #endregion

    public async Task<int> WriteBlobDataAsync(byte[] imageData)
    {

        throw new NotImplementedException();
    }
}