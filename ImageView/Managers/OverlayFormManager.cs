﻿using ImageViewer.Services;
using ImageViewer.UserControls;
using ImageViewer.Utility;

namespace ImageViewer.Managers;

public class OverlayFormManager : IDisposable
{
    private static readonly object LockObject = new();
    private readonly Form _formOverlayImage;
    private readonly ImageSourceAndLocation _imageSourceState;
    private readonly BookmarkPreviewOverlayUserControl _overlayUserControl;
    private int _hideImageDelay;

    private int _showImageDelay;

    public OverlayFormManager(ImageCacheService imageCache)
    {
        _overlayUserControl = new BookmarkPreviewOverlayUserControl(imageCache);
        _formOverlayImage = FormFactory.CreateFloatingForm(_overlayUserControl, new Size(250, 250));
        _overlayUserControl.Dock = DockStyle.Fill;
        _imageSourceState = new ImageSourceAndLocation { LastShownImageTime = DateTime.Today };

        // Default value
        ShowImageDelay = 750;
    }

    public bool IsEnabled { get; set; }
    public int ActiveRow { get; set; }

    public int ShowImageDelay
    {
        get => _showImageDelay;
        set
        {
            if (value >= 0)
            {
                _showImageDelay = value;
            }
        }
    }

    public int HideImageDelay
    {
        get => _hideImageDelay;
        set
        {
            if (value >= 0)
            {
                _hideImageDelay = value;
            }
        }
    }

    public void Dispose()
    {
        _formOverlayImage?.Dispose();
        _overlayUserControl?.Dispose();
        GC.SuppressFinalize(this);
    }

    private void UpdateToLastImageWhenTimeoutExpires()
    {
        lock (LockObject)
        {
            if (_imageSourceState.IsAwaitingDelay)
            {
                return;
            }
        }

        Task.Factory.StartNew(async () =>
        {
            _imageSourceState.IsAwaitingDelay = true;
            bool isMoving = true;

            while (isMoving)
            {
                isMoving = DateTime.Now < _imageSourceState.LastShownImageTime.AddMilliseconds(ShowImageDelay);
                await Task.Delay(HideImageDelay);
            }

            _imageSourceState.IsAwaitingDelay = false;
            if (_imageSourceState.LastShownImagePath != _imageSourceState.ImagePath)
            {
                LoadImageAndDisplayForm(_imageSourceState.ImagePath, _imageSourceState.MousePoint, true);
            }
        });
    }

    public void LoadImageAndDisplayForm(string imagePath, Point mousePoint, bool invokedByOtherThread = false)
    {
        _imageSourceState.ImagePath = imagePath;
        _imageSourceState.MousePointDelta = _imageSourceState.MousePoint;
        _imageSourceState.MousePoint = mousePoint;

        if (_imageSourceState.LastShownImageTime.AddMilliseconds(ShowImageDelay) >= DateTime.Now)
        {
            UpdateToLastImageWhenTimeoutExpires();
            return;
        }

        //Maximize display area
        if (Screen.PrimaryScreen != null)
        {
            var screenBounds = Screen.PrimaryScreen.Bounds;
            if (!_overlayUserControl.LoadImage(imagePath))
            {
                return;
            }


            _imageSourceState.LastShownImagePath = imagePath;
            var imageSize = _overlayUserControl.GetImageSize();

            int maxWidth = Convert.ToInt32(screenBounds.Width / 1.3d);
            int maxHeight = Convert.ToInt32(screenBounds.Height / 1.2d);

            double ratio = imageSize.Width / (double)imageSize.Height;

            if (ratio < 1)
            {
                maxWidth = imageSize.Height > maxHeight ? imageSize.Width : Convert.ToInt32(maxHeight * ratio);
            }
            else
            {
                maxWidth = imageSize.Height > maxHeight ? Convert.ToInt32(maxWidth / ratio) : Convert.ToInt32(maxHeight * ratio);
            }

            var formRectangle = new Rectangle(0, 0, maxWidth, maxHeight);

            if (mousePoint.X > screenBounds.Width / 2)
            {
                formRectangle.Width = Math.Max(maxWidth - mousePoint.X, maxWidth);
                formRectangle.X = mousePoint.X - formRectangle.Width - 10;
            }
            else
            {
                formRectangle.Width = Math.Max(maxWidth - mousePoint.X, maxWidth);
                formRectangle.X = mousePoint.X + 10;
            }

            //Center form on the y axis
            formRectangle.Y = screenBounds.Height / 2 - maxHeight / 2;

            _imageSourceState.LastShownImageTime = DateTime.Now;
            if (!invokedByOtherThread)
            {
                _formOverlayImage.Left = formRectangle.X;
                _formOverlayImage.Top = formRectangle.Y;
                _formOverlayImage.Width = formRectangle.Width;
                _formOverlayImage.Height = formRectangle.Height;
                _formOverlayImage.Show();
            }
        }
    }

    public async Task HideFormWithDelay()
    {
        await Task.Delay(HideImageDelay).ConfigureAwait(true);
        _formOverlayImage.Hide();
    }

    public void HideForm()
    {
        _formOverlayImage.Hide();
    }

    private class ImageSourceAndLocation
    {
        public string ImagePath;
        public bool IsAwaitingDelay;
        public string LastShownImagePath;
        public DateTime LastShownImageTime;
        public Point MousePoint;
        public Point MousePointDelta;
    }
}