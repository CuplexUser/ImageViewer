using System;
using AutoMapper;
using ImageViewer.DataContracts.Import;
using ImageViewer.Models.Import;
using ImageViewer.Providers;
using Serilog;

namespace ImageViewer.Repositories
{
    public class ImageCollectionRepository : RepositoryBase
    {
        private readonly FileSystemIOProvider _fileSystem;
        private readonly IMapper _mapper;

        public ImageCollectionRepository(FileSystemIOProvider fileSystem, IMapper mapper)
        {
            _fileSystem = fileSystem;
            _mapper = mapper;
        }

        public OutputDirectoryModel LoadImageCollection(string fileName)
        {
            try
            {
                SourceFolderDataModel dataModel = _fileSystem.DeserializeObject<SourceFolderDataModel>(fileName);
                var outputDirContainer = _mapper.Map<OutputDirectoryModel>(dataModel);

                return outputDirContainer;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LoadImageCollection failed");
            }

            return null;
        }

        public bool SaveOutputDirectoryModel(string filename, OutputDirectoryModel outputDirectoryModel)
        {
            try
            {
                SourceFolderDataModel dataModel = _mapper.Map<SourceFolderDataModel>(outputDirectoryModel);
                return _fileSystem.SerializeObjectToFile(filename, dataModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "SaveOutputDirectoryModel failed");
            }
            return false;
        }

    }
}