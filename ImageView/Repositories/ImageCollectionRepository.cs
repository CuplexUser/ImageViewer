﻿using ImageViewer.DataContracts.Import;
using ImageViewer.Models.Import;
using ImageViewer.Providers;

namespace ImageViewer.Repositories;

public class ImageCollectionRepository : RepositoryBase
{
    private const string InternalPwd = "a53fKn7XF6Zp&c^$LZt#gtY6fp*b^a3dkWZ&CpwgL4YF%^irJ8";
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
            var dataModel = _fileSystem.DeserializeObject<SourceFolderDataModel>(fileName, InternalPwd);
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
            var dataModel = _mapper.Map<SourceFolderDataModel>(outputDirectoryModel);
            return _fileSystem.SerializeObjectToFile(filename, dataModel, InternalPwd);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "SaveOutputDirectoryModel failed");
        }

        return false;
    }
}