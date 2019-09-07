using System.Collections.Generic;
using ImageView.Services;

namespace ImageView.DataModels
{
    public class ImageReferenceCollection
    {
        private readonly List<int> _randomImagePosList;
        private int _imageListPointer;

        public ImageReferenceCollection(List<int> randomImagePosList)
        {
            _randomImagePosList = randomImagePosList;
        }

        public ImageReferenceElement CurrentImage { get; private set; }

        public int ImageListPointer
        {
            get { return _imageListPointer; }
            set
            {
                if (value < 0)
                    _imageListPointer = _randomImagePosList.Count - 1;
                else if (value >= _randomImagePosList.Count)
                    _imageListPointer = 0;
                else
                    _imageListPointer = value;
            }
        }

        public ImageReferenceElement GetNextImage()
        {
            ImageListPointer = ImageListPointer + 1;
            CurrentImage = ImageLoaderService.Instance.ImageReferenceList[_randomImagePosList[ImageListPointer]];
            return CurrentImage;
        }

        public ImageReferenceElement GetPreviousImage()
        {
            ImageListPointer = ImageListPointer - 1;
            CurrentImage = ImageLoaderService.Instance.ImageReferenceList[_randomImagePosList[ImageListPointer]];
            return CurrentImage;
        }
    }
}