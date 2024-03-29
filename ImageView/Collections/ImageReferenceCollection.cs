﻿using ImageViewer.Library.EventHandlers;
using ImageViewer.Models;
using ImageViewer.Services;

namespace ImageViewer.Collections;

public class ImageReferenceCollection
{
    private readonly ImageLoaderService _imageLoaderService;
    private readonly List<int> _randomImagePosList;
    private int _imageListPointer;

    public ImageReferenceCollection(List<int> randomImagePosList, ImageLoaderService imageLoaderService)
    {
        _randomImagePosList = randomImagePosList;
        _imageLoaderService = imageLoaderService;

        imageLoaderService.OnImportComplete += ImageLoaderService_OnImportComplete;
        imageLoaderService.OnImageWasDeleted += ImageLoaderService_OnImageWasDeleted;
    }

    public ImageReference CurrentImage { get; private set; }

    private int ImageListPointer
    {
        get => _imageListPointer;
        set
        {
            if (value < 0)
            {
                _imageListPointer = _randomImagePosList.Count - 1;
            }
            else if (value >= _randomImagePosList.Count)
            {
                _imageListPointer = 0;
            }
            else
            {
                _imageListPointer = value;
            }
        }
    }

    public int ImageCount => _imageLoaderService.ImageReferenceList?.Count ?? 0;

    private void ImageLoaderService_OnImageWasDeleted(object sender, ImageRemovedEventArgs e)
    {
        // Remove highest imgRefIndex
        int index = e.ImgRefIndexToRemove;
        _randomImagePosList.Remove(index);

        Log.Debug("ImageReference Collection removed highest index from _randomImagePosList. index: {index}", index);

        // to Prevent Argument out of Range Exception in method GetNextImage(), decrement the maximum value in _randomImagePosList by 1
        int maxValue = _randomImagePosList.MaxBy(x => x);
        int i = _randomImagePosList.IndexOf(maxValue);
        _randomImagePosList[i]--;

        if (CurrentImage != null && CurrentImage == e.ImageReference)
        {
            GetNextImage();
        }
    }

    private void ImageLoaderService_OnImportComplete(object sender, ProgressEventArgs e)
    {
        // Create new randomImagePosList based on new import
        _randomImagePosList.Clear();
    }

    public ImageReference GetNextImage()
    {
        if (_randomImagePosList.Count == 0 || _imageLoaderService.ImageReferenceList.Count == 0)
        {
            return null;
        }

        ImageListPointer += 1;
        CurrentImage = _imageLoaderService.ImageReferenceList[_randomImagePosList[ImageListPointer]];
        return CurrentImage;
    }

    public ImageReference PeekNextImage()
    {
        int index = ImageListPointer + 1;
        if (index >= _randomImagePosList.Count)
        {
            index = 0;
        }

        return _imageLoaderService.ImageReferenceList[_randomImagePosList[index]];
    }

    public ImageReference GetPreviousImage()
    {
        ImageListPointer = ImageListPointer - 1;
        CurrentImage = _imageLoaderService.ImageReferenceList[_randomImagePosList[ImageListPointer]];
        return CurrentImage;
    }

    public void SingleImageLoadedSetAsCurrent()
    {
        if (_randomImagePosList == null)
        {
            Log.Error("ImageReference Collection:SingleImageLoadedSetAsCurrent() Assert error _randomImagePosList==null");
            return;
        }

        while (_randomImagePosList.Count < _imageLoaderService.ImageReferenceList.Count)
        {
            int maxValue = _randomImagePosList.Max();
            _randomImagePosList.Add(maxValue + 1);
        }

        ImageListPointer = _imageLoaderService.ImageReferenceList.Count - 1;
        CurrentImage = _imageLoaderService.ImageReferenceList[_randomImagePosList[ImageListPointer]];
    }
}