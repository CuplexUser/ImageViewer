using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using GeneralToolkitLib.Hashing;

namespace ImageViewer.Providers;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class BlobStorageProvider : ProviderBase, IDisposable, IEqualityComparer<BlobStorageProvider>
{
    private const string BlobStorageFileName = "thumbnailStorage.bin";

    private static readonly byte[] HeaderBytes =
    {
        0x33, 0xB2, 0xC1, 0xDF, 0x23, 0xA2, 0xC9, 0x66, 0x73, 0xA6, 0x85, 0x8F, 0xE6, 0xA1, 0x06, 0x2E, 0xE8, 0xA9,
        0x39, 0x76, 0xFB, 0x83, 0xE1, 0xF3, 0x2B, 0xF6, 0x19, 0x1D, 0xCC, 0x0C, 0xE1, 0xF
    };

    private readonly string _blobStorageFilename;
    private readonly ReaderWriterLockSlim _readerWriterLock;

    public readonly string InstanceId;

    private readonly object LockObject = new();
    private FileStream _blobDataFileStream;

    private long filePosition;
    private int WriteCount;

    public BlobStorageProvider()
    {
        _blobStorageFilename = Path.Join(GlobalSettings.Instance.GetUserDataDirectoryPath(), BlobStorageFileName);
        InstanceId = SHA256.GetSHA256HashAsHexString(_blobStorageFilename);
        _readerWriterLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
    }

    // Use a custom file name
    public BlobStorageProvider(string fileName)
    {
        _blobStorageFilename = Path.Join(GlobalSettings.Instance.GetUserDataDirectoryPath(), fileName);
        InstanceId = SHA256.GetSHA256HashAsHexString(_blobStorageFilename);
        _readerWriterLock = new ReaderWriterLockSlim();
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
        byte[] buffer = new byte[length];
        int bytesRead;

        filePosition = position;
        //_readerWriterLock.TryEnterReadLock(2000);

        try
        {
            if (!_blobDataFileStream.CanRead)
            {
                if (!OpenStorageFile())
                {
                    return null;
                }
            }

            if (position < HeaderBytes.Length)
            {
                position = HeaderBytes.Length;
                filePosition = position;
                Log.Warning("Tried to read data before data blocks begin");
            }

            _blobDataFileStream.Position = position;

            // ReadAsync gives terrible performance and the file stream is thread safe so using readerWriterLock
            // when writing will work without any problem.
            bytesRead = _blobDataFileStream.Read(buffer, 0, length);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "ReadBlobDataAsync failed");
            return null;
        }
        finally
        {
            await Task.Yield();
        }

        return bytesRead != buffer.Length ? null : buffer;
    }

    public int WriteBlobData(byte[] data)
    {
        if (!StorageFileIsOpen)
        {
            lock (LockObject)
            {
                OpenStorageFile();
            }
        }

        int currentFilePosition = -1;
        if (_readerWriterLock.TryEnterWriteLock(TimeSpan.FromMilliseconds(2000)))
        {
            try
            {
                Interlocked.Increment(ref WriteCount);
                currentFilePosition = Convert.ToInt32(_blobDataFileStream.Length);
                _blobDataFileStream.Position = currentFilePosition;

                // Offset refers to offset in the array
                _blobDataFileStream.Write(data, 0, data.Length);
            }
            finally
            {
                Interlocked.Decrement(ref WriteCount);
                _readerWriterLock.ExitWriteLock();
            }
        }

        return currentFilePosition;
    }

    public async Task<int> WriteBlobDataAsync(byte[] imageData)
    {
        if (!StorageFileIsOpen)
        {
            lock (LockObject)
            {
                OpenStorageFile();
            }
        }

        int currentFilePosition = -1;
        try
        {
            _readerWriterLock.EnterWriteLock();
            await new Task(() =>
            {
                Interlocked.Increment(ref WriteCount);
                currentFilePosition = Convert.ToInt32(_blobDataFileStream.Length);
                _blobDataFileStream.Position = currentFilePosition;

                // Offset refers to offset in the array
                _blobDataFileStream.Write(imageData, 0, imageData.Length);
                return;
            });


        }
        finally
        {
            Interlocked.Decrement(ref WriteCount);
            _readerWriterLock.ExitWriteLock();
        }


        return currentFilePosition;
    }


    private bool CreateNewBlobStorageFile()
    {
        if (_readerWriterLock.TryEnterWriteLock(5000))
        {
            try
            {
                Interlocked.Increment(ref WriteCount);
                _blobDataFileStream = File.Open(_blobStorageFilename, FileMode.CreateNew);
                _blobDataFileStream.Position = 0;
                _blobDataFileStream.Write(HeaderBytes);
                _blobDataFileStream.Flush();

                return true;
            }
            finally
            {
                Interlocked.Decrement(ref WriteCount);
                _readerWriterLock.ExitWriteLock();
            }
        }

        return false;
    }

    public long GetFileSize()
    {
        if (StorageFileIsOpen)
        {
            return _blobDataFileStream.Length;
        }

        return 0;
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
            _readerWriterLock.EnterWriteLock();

            if (_blobDataFileStream != null && _blobDataFileStream.Length > 0)
            {
                _blobDataFileStream.Flush(true);
                _blobDataFileStream.Close();
                _blobDataFileStream = null;
            }

            if (File.Exists(_blobStorageFilename))
            {
                File.Delete(_blobStorageFilename);
            }
        }
        finally
        {
            _readerWriterLock.ExitWriteLock();
        }

        return OpenStorageFile();
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
}