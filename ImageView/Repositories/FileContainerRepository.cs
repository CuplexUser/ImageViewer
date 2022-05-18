using AutoMapper;
using ImageViewer.Managers;
using ImageViewer.Models;
using System.Collections.Concurrent;

namespace ImageViewer.Repositories
{
    public abstract class FileContainerRepository
    {
        protected virtual string DatabaseFilename { get; } = "thumbs.ibd";
        private protected readonly ReaderWriterLockSlim _readerWriterLock;
        protected readonly IMapper _mapper;
        private readonly string _indexDbPath;
        private readonly string _binaryDataPath;
        private readonly FileManager _fileManager;
        private readonly ConcurrentDictionary<string, ThumbnailEntry> _thumbnailEntries;

        protected FileContainerRepository(IMapper mapper, string indexDbPath, string binaryDataPath, FileManager fileManager)
        {
            _mapper = mapper;
            _indexDbPath = indexDbPath;
            _binaryDataPath = binaryDataPath;
            _fileManager = fileManager;
            _thumbnailEntries = new ConcurrentDictionary<string, ThumbnailEntry>();
            _readerWriterLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }


        public virtual ThumbnailEntry GeThumbnailEntry(string fullPath)
        {
            _thumbnailEntries.TryGetValue(fullPath, out var thumbnailEntry);
            return thumbnailEntry;
        }

        public virtual bool AddThumbnailEntry(string fullPath, ThumbnailEntry thumbnailEntry)
        {
            var result = _thumbnailEntries.AddOrUpdate(fullPath, thumbnailEntry, (s, entry) => new ThumbnailEntry());
            return true;
        }

        protected abstract bool LoadDatabase();
        public abstract bool SaveDatabase();
    }
}