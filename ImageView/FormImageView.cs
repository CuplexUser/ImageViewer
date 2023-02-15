using System.Drawing.Drawing2D;
using GeneralToolkitLib.WindowsApi;
using ImageViewer.Collections;
using ImageViewer.Events;
using ImageViewer.Interfaces;
using ImageViewer.Library.EventHandlers;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Properties;
using ImageViewer.Services;
using ImageViewer.Utility;

namespace ImageViewer;

/// <summary>
/// </summary>
/// <seealso cref="System.Windows.Forms.Form" />
/// <seealso cref="System.IObservable&lt;ImageViewer.Events.ImageViewFormInfoBase&gt;" />
/// <seealso cref="ImageViewer.Interfaces.IMageViewFormWindow" />
public partial class FormImageView : Form, IObservable<ImageViewFormInfoBase>, IMageViewFormWindow
{
    /// <summary>
    ///     The zoom minimum
    /// </summary>
    private const float ZoomMin = 0.0095f;

    /// <summary>
    ///     The change image panel width
    /// </summary>
    private const int ChangeImagePanelWidth = 50;

    /// <summary>
    ///     The application settings service
    /// </summary>
    private readonly ApplicationSettingsService _applicationSettingsService;

    /// <summary>
    ///     The bookmark manager
    /// </summary>
    private readonly BookmarkManager _bookmarkManager;

    /// <summary>
    ///     The form add bookmark
    /// </summary>
    private readonly FormAddBookmark _formAddBookmark;

    /// <summary>
    ///     The image cache
    /// </summary>
    private readonly ImageCacheService _imageCache;

    /// <summary>
    ///     The image loader service
    /// </summary>
    private readonly ImageLoaderService _imageLoaderService;

    /// <summary>
    ///     The observers
    /// </summary>
    private readonly List<IObserver<ImageViewFormInfoBase>> _observers;

    /// <summary>
    ///     The current image
    /// </summary>
    private Image _currentImage;

    /// <summary>
    ///     The data ready
    /// </summary>
    private bool _dataReady;

    /// <summary>
    ///     The image reference collection
    /// </summary>
    private ImageReferenceCollection _imageReferenceCollection;

    /// <summary>
    ///     The images viewed
    /// </summary>
    private int _imagesViewed;

    /// <summary>
    ///     The image view form information
    /// </summary>
    private ImageViewFormImageInfo _imageViewFormInfo;

    /// <summary>
    ///     The img reference
    /// </summary>
    private ImageReference _imgRef;

    /// <summary>
    ///     The imgx
    /// </summary>
    private int _imgx; // current offset of image

    /// <summary>
    ///     The imgy
    /// </summary>
    private int _imgy;

    /// <summary>
    ///     The last form state
    /// </summary>
    private FormWindowState _lastFormState;

    /// <summary>
    ///     The mouse down
    /// </summary>
    private Point _mouseDown;

    /// <summary>
    ///     The mouse hover
    /// </summary>
    private bool _mouseHover;

    /// <summary>
    ///     The mouse hover information
    /// </summary>
    private MouseHoverInfo _mouseHoverInfo;

    /// <summary>
    ///     The mouse pressed
    /// </summary>
    private bool _mousePressed; // true as long as left mouse button is pressed

    /// <summary>
    ///     The require focus notification
    /// </summary>
    private bool _requireFocusNotification = true;

    /// <summary>
    ///     The show switch img on mouse over window
    /// </summary>
    private bool _showSwitchImgOnMouseOverWindow;

    /// <summary>
    ///     The show switch img panel
    /// </summary>
    private bool _showSwitchImgPanel;

    /// <summary>
    ///     The start x
    /// </summary>
    private int _startX; // offset of image when mouse was pressed

    /// <summary>
    ///     The start y
    /// </summary>
    private int _startY;

    /// <summary>
    ///     The switch img buttons enabled
    /// </summary>
    private bool _switchImgButtonsEnabled;

    /// <summary>
    ///     The zoom
    /// </summary>
    private double _zoom = -1.0;

    /// <summary>
    ///     Initializes a new instance of the <see cref="FormImageView" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="formAddBookmark">The form add bookmark.</param>
    /// <param name="bookmarkManager">The bookmark manager.</param>
    /// <param name="applicationSettingsService">The application settings service.</param>
    /// <param name="imageCache">The image cache.</param>
    /// <param name="imageLoaderService">The image loader service.</param>
    public FormImageView(int id, FormAddBookmark formAddBookmark, BookmarkManager bookmarkManager, ApplicationSettingsService applicationSettingsService,
        ImageCacheService imageCache, ImageLoaderService imageLoaderService)
    {
        InitializeComponent();
        _imageViewFormInfo = new ImageViewFormImageInfo(this, null, 0);
        _observers = new List<IObserver<ImageViewFormInfoBase>>();
        pictureBox.Paint += pictureBox_Paint;
        FormId = id;
        _formAddBookmark = formAddBookmark;
        _bookmarkManager = bookmarkManager;
        _applicationSettingsService = applicationSettingsService;
        _imageCache = imageCache;
        _imageLoaderService = imageLoaderService;
        _lastFormState = WindowState;

        imageLoaderService.OnImportComplete += ImageLoaderService_OnImportComplete;
        imageLoaderService.OnImageWasDeleted += ImageLoaderService_OnImageWasDeleted;

        ReloadSettings();
    }

    /// <summary>
    ///     Gets the form identifier.
    /// </summary>
    /// <value>
    ///     The form identifier.
    /// </value>
    private int FormId { get; }

    /// <summary>
    ///     Gets a value indicating whether [image source data available].
    /// </summary>
    /// <value>
    ///     <c>true</c> if [image source data available]; otherwise, <c>false</c>.
    /// </value>
    private bool ImageSourceDataAvailable => _dataReady && _imageLoaderService.ImageReferenceList != null && !_imageLoaderService.IsRunningImport;

    /// <summary>
    ///     Resets the zoom and repaint.
    /// </summary>
    public void ResetZoomAndRepaint()
    {
        //Center Image and resize
        _imgx = 0;
        _imgy = 0;
        ResetZoom(true);
        pictureBox.Refresh();
    }

    /// <summary>
    ///     Reloads the settings.
    /// </summary>
    public void ReloadSettings()
    {
        _switchImgButtonsEnabled = _applicationSettingsService.Settings.ShowSwitchImageButtons;
        _showSwitchImgOnMouseOverWindow = _applicationSettingsService.Settings.ShowNextPrevControlsOnEnterWindow;
        pictureBox.BackColor = _applicationSettingsService.Settings.MainWindowBackgroundColor;
        _mouseHoverInfo = _switchImgButtonsEnabled ? new MouseHoverInfo() : null;
    }

    /// <summary>
    ///     Notifies the provider that an observer is to receive notifications.
    /// </summary>
    /// <param name="observer">The object that is to receive notifications.</param>
    /// <returns>
    ///     A reference to an interface that allows observers to stop receiving notifications before the provider has finished
    ///     sending them.
    /// </returns>
    public IDisposable Subscribe(IObserver<ImageViewFormInfoBase> observer)
    {
        // Check whether observer is already registered. If not, add it 
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
            // Provide observer with existing data. 
            observer.OnNext(_imageViewFormInfo);
        }


        _imageLoaderService.OnImportComplete -= ImageLoaderService_OnImportComplete;
        _imageLoaderService.OnImageWasDeleted -= ImageLoaderService_OnImageWasDeleted;


        return (observer as FormWindows)!;
        //return new Unsubscriber<ImageViewFormInfoBase>(_observers, observer);
    }


    /// <summary>
    ///     Handles the OnImportComplete event of the ImageLoaderService control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="ProgressEventArgs" /> instance containing the event data.</param>
    private void ImageLoaderService_OnImportComplete(object sender, ProgressEventArgs e)
    {
        SetImageReferenceCollection();
        if (!ImageSourceDataAvailable)
        {
            return;
        }

        _imgRef = _imageReferenceCollection.GetNextImage();
        LoadNewImageFile(_imgRef);
    }

    /// <summary>
    ///     Handles the OnImageWasDeleted event of the ImageLoaderService control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="ImageRemovedEventArgs" /> instance containing the event data.</param>
    private void ImageLoaderService_OnImageWasDeleted(object sender, ImageRemovedEventArgs e)
    {
    }

    /// <summary>
    ///     Raises the <see cref="E:System.Windows.Forms.Control.ClientSizeChanged" /> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnClientSizeChanged(EventArgs e)
    {
        if (WindowState != _lastFormState)
        {
            _lastFormState = WindowState;
            WindowStateChanged(e);
        }
    }

    /// <summary>
    ///     Windows the state changed.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected void WindowStateChanged(EventArgs e)
    {
        ResetZoomAndRepaint();
    }

    /// <summary>
    ///     Handles the Load event of the FormImageView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void FormImageView_Load(object sender, EventArgs e)
    {
        //pictureBox.BackColor = _applicationSettingsService.Settings.MainWindowBackgroundColor.ToColor();
        ShowInTaskbar = _applicationSettingsService.Settings.ShowImageViewFormsInTaskBar;
        SetImageReferenceCollection();
        if (!ImageSourceDataAvailable)
        {
            return;
        }

        _imgRef = _imageReferenceCollection.GetNextImage();
        LoadNewImageFile(_imgRef);
    }

    /// <summary>
    ///     Sets the image reference collection.
    /// </summary>
    private void SetImageReferenceCollection()
    {
        bool randomizeImageCollection = _applicationSettingsService.Settings.AutoRandomizeCollection;
        if (!_imageLoaderService.IsRunningImport && _imageLoaderService.ImageReferenceList != null)
        {
            _imageReferenceCollection = _imageLoaderService.GenerateImageReferenceCollection(randomizeImageCollection);
            _dataReady = true;
        }
    }

    /// <summary>
    ///     Loads the new image file.
    /// </summary>
    /// <param name="imageReference">The image reference.</param>
    private void LoadNewImageFile(ImageReference imageReference)
    {
        try
        {
            _currentImage = _imageCache.GetImageFromCache(imageReference.CompletePath);

            //_imgx = 0;
            //_imgy = 0;
            ResetZoom(true);

            pictureBox.Refresh();
            Text = imageReference.FileName;

            //Notify observers
            _imagesViewed++;
            _imageViewFormInfo = new ImageViewFormImageInfo(this, imageReference.FileName, _imagesViewed);
            foreach (var observer in _observers) observer.OnNext(_imageViewFormInfo);

            Log.Verbose("New Image loaded in ImageViewForm FormId = {FormId}", FormId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "FormMain.LoadNewImageFile(string imagePath) Error when trying to load file: {CompletePath}, Image Ref is null ({IsNull})", imageReference?.CompletePath,
                imageReference == null);
        }
    }

    /// <summary>
    ///     Handles the FormClosed event of the FormImageView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="FormClosedEventArgs" /> instance containing the event data.</param>
    private void FormImageView_FormClosed(object sender, FormClosedEventArgs e)
    {
        _imageViewFormInfo.FormIsClosing = true;
        foreach (var observer in _observers) observer.OnNext(_imageViewFormInfo);

        Log.Verbose("ImageView Form with id={FormId} closed", FormId);
    }

    /// <summary>
    ///     Handles the MouseDown event of the pictureBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
    private void pictureBox_MouseDown(object sender, MouseEventArgs e)
    {
        var mouse = e;
        if (mouse.Button == MouseButtons.Left)
        {
            if (_mousePressed)
            {
                return;
            }

            _mousePressed = true;
            _mouseDown = mouse.Location;
            _startX = _imgx;
            _startY = _imgy;
        }
    }

    /// <summary>
    ///     Handles the MouseMove event of the pictureBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
    private void pictureBox_MouseMove(object sender, MouseEventArgs e)
    {
        var mouse = e;

        if (mouse.Button == MouseButtons.Left)
        {
            var mousePosNow = mouse.Location;

            int deltaX = mousePosNow.X - _mouseDown.X;
            // the distance the mouse has been moved since mouse was pressed
            int deltaY = mousePosNow.Y - _mouseDown.Y;

            _imgx = (int)(_startX + deltaX / _zoom);
            // calculate new offset of image based on the current zoom factor
            _imgy = (int)(_startY + deltaY / _zoom);

            pictureBox.Refresh();
        }

        if (_mouseHoverInfo == null)
        {
            return;
        }

        var leftPanel = new Rectangle(0, 0, ChangeImagePanelWidth, Height);
        var rightPanel = new Rectangle(ClientSize.Width - ChangeImagePanelWidth, 0, ChangeImagePanelWidth, Height);

        _mouseHoverInfo.OverLeftPanel = false;
        _mouseHoverInfo.OverRightPanel = false;
        _mouseHoverInfo.LeftButtonPressed = mouse.Button == MouseButtons.Left;

        if (leftPanel.IntersectsWith(new Rectangle(mouse.Location, new Size(1, 1))))
        {
            _mouseHoverInfo.OverLeftPanel = true;
        }
        else if (rightPanel.IntersectsWith(new Rectangle(mouse.Location, new Size(1, 1))))
        {
            _mouseHoverInfo.OverRightPanel = true;
        }

        UpdateSwitchImgPanelState();


        if (_mouseHoverInfo.StateChanged)
        {
            pictureBox.Refresh();
            _mouseHoverInfo.ResetState();
        }
    }

    /// <summary>
    ///     Updates the state of the switch img panel.
    /// </summary>
    private void UpdateSwitchImgPanelState()
    {
        _showSwitchImgPanel = _switchImgButtonsEnabled && _mouseHover;
    }

    /// <summary>
    ///     Handles the MouseUp event of the pictureBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
    private void pictureBox_MouseUp(object sender, MouseEventArgs e)
    {
        _mousePressed = false;
    }

    /// <summary>
    ///     Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnResize(EventArgs e)
    {
        ResetZoom(true);
        base.OnResize(e);
        pictureBox.Refresh();
    }

    /// <summary>
    ///     Processes a command key.
    /// </summary>
    /// <param name="msg">
    ///     A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the Win32
    ///     message to process.
    /// </param>
    /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process.</param>
    /// <returns>
    ///     <see langword="true" /> if the keystroke was processed and consumed by the control; otherwise,
    ///     <see langword="false" /> to allow further processing.
    /// </returns>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (msg.Msg != WindowEvents.WM_KEYDOWN && msg.Msg != WindowEvents.WM_SYSKEYDOWN)
        {
            return base.ProcessCmdKey(ref msg, keyData);
        }

        switch (keyData)
        {
            case Keys.Right:
                _imgx -= (int)(pictureBox.Width * 0.1F / _zoom);
                pictureBox.Refresh();
                break;

            case Keys.Left:
                _imgx += (int)(pictureBox.Width * 0.1F / _zoom);
                pictureBox.Refresh();
                break;

            case Keys.Down:
                _imgy -= (int)(pictureBox.Height * 0.1F / _zoom);
                pictureBox.Refresh();
                break;

            case Keys.Up:
                _imgy += (int)(pictureBox.Height * 0.1F / _zoom);
                pictureBox.Refresh();
                break;

            case Keys.PageDown:
                _imgy -= (int)(pictureBox.Height * 0.90F / _zoom);
                pictureBox.Refresh();
                break;

            case Keys.PageUp:
                _imgy += (int)(pictureBox.Height * 0.90F / _zoom);
                pictureBox.Refresh();
                break;

            case Keys.NumPad1:
            case Keys.D1:
            case Keys.Add:
                SetNextImage();
                break;

            case Keys.NumPad2:
            case Keys.D2:
            case Keys.Subtract:
                SetPreviousImage();
                break;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>
    ///     Sets the next image.
    /// </summary>
    private void SetNextImage()
    {
        if (!ImageSourceDataAvailable)
        {
            //Try SetImageReference collection
            SetImageReferenceCollection();
            return;
        }

        _imgRef = _imageReferenceCollection.GetNextImage();
        LoadNewImageFile(_imgRef);
    }

    /// <summary>
    ///     Sets the previous image.
    /// </summary>
    private void SetPreviousImage()
    {
        if (!ImageSourceDataAvailable)
        {
            //Try SetImageReference collection
            SetImageReferenceCollection();
            return;
        }

        _imgRef = _imageReferenceCollection.GetPreviousImage();
        LoadNewImageFile(_imgRef);
    }

    /// <summary>
    ///     Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
    protected override void OnMouseWheel(MouseEventArgs e)
    {
        double oldzoom = _zoom;

        if (e.Delta > 0)
        {
            _zoom += 0.1F + _zoom * .05f;
        }

        else if (e.Delta < 0)
        {
            _zoom = Math.Max(_zoom - 0.1F - _zoom * .05f, ZoomMin);
        }

        var mouse = e;
        var mousePosNow = mouse.Location;

        int x = mousePosNow.X - pictureBox.Location.X; // Where location of the mouse in the pictureframe
        int y = mousePosNow.Y - pictureBox.Location.Y;

        int oldimagex = (int)(x / oldzoom); // Where in the IMAGE is it now
        int oldimagey = (int)(y / oldzoom);

        int newimagex = (int)(x / _zoom); // Where in the IMAGE will it be when the new zoom i made
        int newimagey = (int)(y / _zoom);

        _imgx = newimagex - oldimagex + _imgx; // Where to move image to keep focus on one point
        _imgy = newimagey - oldimagey + _imgy;

        if (_zoom < ZoomMin)
        {
            _zoom = ZoomMin;
        }

        pictureBox.Refresh();
    }

    /// <summary>
    ///     Resets the zoom.
    /// </summary>
    /// <param name="fitEntireImage">if set to <c>true</c> [fit entire image].</param>
    private void ResetZoom(bool fitEntireImage)
    {
        _imgx = 0;
        _imgy = 0;

        if (_currentImage == null)
        {
            return;
        }

        var constraints = new ImageConstraints
        {
            ImageConstraint = _currentImage.Width > _currentImage.Height ? Constraint.Height : Constraint.Width,
            WindowConstraint = pictureBox.Width > pictureBox.Height ? Constraint.Height : Constraint.Width,
            ImageSize = _currentImage.Size,
            WindowSize = pictureBox.Size
        };


        // Bugfix for:
        // Exception when right bottom corner is clicked and for one ore more windows we get an exception on the int parse call below:
        // (Value was either too large or too small for an Int32.)
        if (WindowState != FormWindowState.Normal || pictureBox.Height < 200 || pictureBox.Width < 200)
        {
            return;
        }

        var g = CreateGraphics();

        double vResAdjustment = _currentImage.VerticalResolution / g.DpiY;
        double hResAdjustment = _currentImage.HorizontalResolution / g.DpiX;

        double scaleFactorWindow = constraints.WindowSize.Width / (double)constraints.WindowSize.Height;
        double scaleFactorImage = constraints.ImageSize.Width / (double)constraints.ImageSize.Height;

        if (fitEntireImage)
        {
            // Wide Window
            if (constraints.WindowConstraint == Constraint.Height)
            {
                // Wide Image
                if (constraints.ImageConstraint == Constraint.Height)
                {
                    _zoom = constraints.WindowSize.Height / (double)constraints.ImageSize.Height;
                    _imgx = Convert.ToInt32(constraints.WindowSize.Width - constraints.ImageSize.Width / 2d * _zoom);
                }
                else
                {
                    //Narrow image - Wide window
                    int height = constraints.WindowSize.Height;
                    _zoom = height / (double)constraints.ImageSize.Height;
                    _imgx = Convert.ToInt32((constraints.WindowSize.Width - constraints.ImageSize.Width * _zoom) / 2d);
                }

                _zoom *= vResAdjustment;
            }
            else
            {
                //Narrow Window
                // Wide Image
                if (constraints.ImageConstraint == Constraint.Height)
                {
                    int width = constraints.WindowSize.Width;
                    _zoom = width / (double)constraints.ImageSize.Width;
                    _imgy = Convert.ToInt32((constraints.WindowSize.Height - constraints.ImageSize.Height * _zoom) / 2d);
                }
                else
                {
                    //Narrow Window
                    // Narrow Image
                    if (scaleFactorWindow > scaleFactorImage)
                    {
                        int height = constraints.WindowSize.Height;
                        _zoom = height / (double)constraints.ImageSize.Height;
                        _imgx = Convert.ToInt32((constraints.WindowSize.Width - constraints.ImageSize.Width * _zoom) / 2d);
                    }
                    else
                    {
                        int width = constraints.WindowSize.Width;
                        _zoom = width / (double)constraints.ImageSize.Width;
                        _imgy = Convert.ToInt32((constraints.WindowSize.Height - constraints.ImageSize.Height * _zoom) / 2d);
                    }
                }

                _zoom *= vResAdjustment;
            }
        }
        else
        {
            _zoom = pictureBox.Width / (float)_currentImage.Width * (_currentImage.HorizontalResolution / g.DpiX);
        }

        pictureBox.Refresh();
    }

    /// <summary>
    ///     Handles the Paint event of the pictureBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="PaintEventArgs" /> instance containing the event data.</param>
    private void pictureBox_Paint(object sender, PaintEventArgs e)
    {
        if (_currentImage == null || _zoom <= 0)
        {
            return;
        }

        try
        {
            float scaleFactor = (float)_zoom;
            var g = e.Graphics;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.ScaleTransform(scaleFactor, scaleFactor);
            g.DrawImage(_currentImage, _imgx / scaleFactor, _imgy / scaleFactor);

            //var drawRect = pictureBox.ClientRectangle;
            //drawRect.Size= pictureBox.Size with {Height = 40};
            //drawRect.Y = pictureBox.ClientRectangle.Height - 40;
            //g.ResetTransform();
            //g.FillRectangle(new SolidBrush(Color.Blue), drawRect);
            g.ResetTransform();

            if (_showSwitchImgPanel)
            {
                if (!_showSwitchImgOnMouseOverWindow && !_mouseHoverInfo.OverAnyButton)
                {
                    return;
                }

                g.ResetTransform();
                Brush b = new SolidBrush(Color.FromArgb(64, Color.LightGray));

                for (int i = 0; i < 2; i++)
                {
                    g.FillRectangle(b, new Rectangle(0, 0, ChangeImagePanelWidth, ClientSize.Height));
                    g.FillRectangle(b, new Rectangle(ClientSize.Width - ChangeImagePanelWidth, 0, ChangeImagePanelWidth, ClientSize.Height));
                    b = new SolidBrush(Color.FromArgb(128, Color.Black));
                }

                int imgWidth = Convert.ToInt32(Math.Min(Resources.Arrow_Back_icon.Width, ChangeImagePanelWidth) * 0.8);
                float imgScale = (float)imgWidth / Resources.Arrow_Back_icon.Width * 0.7f;
                int imgMargin = (ChangeImagePanelWidth - imgWidth) / 2;
                int imgYpos = ClientSize.Height / 2 - imgWidth / 2;

                g.ScaleTransform(imgScale, imgScale);
                g.TranslateTransform(imgMargin, 0);
                var leftArrowPos = new Point(0, imgYpos);
                var rightArrowPos = new Point(ClientSize.Width - ChangeImagePanelWidth, imgYpos);

                g.DrawImage(Resources.Arrow_Back_icon, TranslatePoint(leftArrowPos, imgScale));
                g.DrawImage(Resources.Arrow_Next_icon, TranslatePoint(rightArrowPos, imgScale));
                g.ResetTransform();

                if (_mouseHoverInfo.OverAnyButton)
                {
                    var rect = _mouseHoverInfo.OverLeftPanel
                        ? new Rectangle(0, 0, ChangeImagePanelWidth, Height)
                        : new Rectangle(ClientSize.Width - ChangeImagePanelWidth - 1, 0, ChangeImagePanelWidth, Height);
                    Brush selectionBrush = new HatchBrush(HatchStyle.Percent50, Color.DimGray);

                    var p = new Pen(selectionBrush);
                    g.DrawRectangle(p, rect);
                    rect.Inflate(-1, -1);
                    p.Color = Color.FromArgb(128, Color.MidnightBlue);
                    g.DrawRectangle(p, rect);

                    b = new SolidBrush(Color.FromArgb(96, Color.Black));
                    g.FillRectangle(b, rect);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Exception in image scale transform");
        }
    }

    /// <summary>
    ///     Translates the point.
    /// </summary>
    /// <param name="point">The point.</param>
    /// <param name="scale">The scale.</param>
    /// <returns></returns>
    private PointF TranslatePoint(Point point, float scale)
    {
        return new PointF(point.X / scale, point.Y / scale);
    }

    /// <summary>
    ///     Handles the Click event of the copyFilepathToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void copyFilepathToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (_imgRef != null)
        {
            Clipboard.Clear();
            Clipboard.SetText(_imgRef.CompletePath);
        }
    }

    /// <summary>
    ///     Handles the Click event of the openWithDefaultProgramToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void openWithDefaultProgramToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (_imgRef == null)
        {
            return;
        }

        ApplicationIOHelper.OpenImageInDefaultAplication(_imgRef.CompletePath);
    }

    /// <summary>
    ///     Handles the Activated event of the FormImageView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void FormImageView_Activated(object sender, EventArgs e)
    {
        _requireFocusNotification = true;

        if (Focused && _requireFocusNotification)
        {
            var formFocusStateInfo = new ImageViewFormInfo(this);
            foreach (var observer in _observers) observer.OnNext(formFocusStateInfo);
        }
    }

    /// <summary>
    ///     Handles the Deactivate event of the FormImageView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void FormImageView_Deactivate(object sender, EventArgs e)
    {
        if (!Focused)
        {
            _requireFocusNotification = true;
            var formFocusStateInfo = new ImageViewFormInfo(this, true);
            foreach (var observer in _observers) observer.OnNext(formFocusStateInfo);
        }
    }

    /// <summary>
    ///     Handles the MouseEnter event of the pictureBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void pictureBox_MouseEnter(object sender, EventArgs e)
    {
        if (!_switchImgButtonsEnabled)
        {
            return;
        }

        _mouseHover = true;
        UpdateSwitchImgPanelState();
        pictureBox.Refresh();
    }

    /// <summary>
    ///     Handles the MouseLeave event of the pictureBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void pictureBox_MouseLeave(object sender, EventArgs e)
    {
        if (!_switchImgButtonsEnabled)
        {
            return;
        }

        _mouseHover = false;
        UpdateSwitchImgPanelState();
        pictureBox.Refresh();
    }

    /// <summary>
    ///     Handles the MouseClick event of the pictureBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
    private void pictureBox_MouseClick(object sender, MouseEventArgs e)
    {
        if (_mouseHoverInfo != null && _mouseHoverInfo.OverAnyButton && _showSwitchImgPanel)
        {
            if (_mouseHoverInfo.OverLeftPanel)
            {
                SetPreviousImage();
            }
            else
            {
                SetNextImage();
            }
        }
    }

    /// <summary>
    ///     Handles the Click event of the bookmarkImageToolStripMenuItem control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void bookmarkImageToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (_bookmarkManager == null)
        {
            MessageBox.Show(Resources.Please_unlock_bookmarks_first);
            return;
        }

        if (!ImageSourceDataAvailable || _imageReferenceCollection.CurrentImage == null)
        {
            return;
        }

        var starupPosition = Location;
        _formAddBookmark.Init(starupPosition, _imageReferenceCollection.CurrentImage);
        _formAddBookmark.ShowDialog(this);
    }

    /// <summary>
    ///     Handles the Resize event of the FormImageView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void FormImageView_Resize(object sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Normal && _currentImage != null)
        {
            _imgx = 0;
            _imgy = 0;
            ResetZoom(true);
        }
    }

    /// <summary>
    ///     Handles the SizeChanged event of the FormImageView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    private void FormImageView_SizeChanged(object sender, EventArgs e)
    {
    }


    /// <summary>
    ///     Constraint - The Smalest component of a rectangle, determening how to scale an image
    /// </summary>
    private enum Constraint
    {
        /// <summary>
        ///     The width
        /// </summary>
        Width, // Width is less than height

        /// <summary>
        ///     The height
        /// </summary>
        Height // Height is less than Width
    }

    /// <summary>
    ///     The constraints of the rendered Image, where 4 components deside what to maximize and what position to set
    /// </summary>
    private class ImageConstraints
    {
        /// <summary>
        ///     The image constraint
        /// </summary>
        public Constraint ImageConstraint;

        /// <summary>
        ///     The image size
        /// </summary>
        public Size ImageSize;

        /// <summary>
        ///     The window constraint
        /// </summary>
        public Constraint WindowConstraint;

        /// <summary>
        ///     The window size
        /// </summary>
        public Size WindowSize;
    }

    /// <summary>
    /// </summary>
    private class MouseHoverInfo
    {
        /// <summary>
        ///     The left button pressed
        /// </summary>
        private bool _leftButtonPressed;

        /// <summary>
        ///     The over left button
        /// </summary>
        private bool _overLeftButton;

        /// <summary>
        ///     The over right button
        /// </summary>
        private bool _overRightButton;

        /// <summary>
        ///     Gets or sets a value indicating whether [over left panel].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [over left panel]; otherwise, <c>false</c>.
        /// </value>
        public bool OverLeftPanel
        {
            get => _overLeftButton;
            set
            {
                if (_overLeftButton != value)
                {
                    StateChanged = true;
                }

                _overLeftButton = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [over right panel].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [over right panel]; otherwise, <c>false</c>.
        /// </value>
        public bool OverRightPanel
        {
            get => _overRightButton;
            set
            {
                if (_overRightButton != value)
                {
                    StateChanged = true;
                }

                _overRightButton = value;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether [over any button].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [over any button]; otherwise, <c>false</c>.
        /// </value>
        public bool OverAnyButton => OverLeftPanel || OverRightPanel;

        /// <summary>
        ///     Gets or sets a value indicating whether [state changed].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [state changed]; otherwise, <c>false</c>.
        /// </value>
        public bool StateChanged { get; private set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [left button pressed].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [left button pressed]; otherwise, <c>false</c>.
        /// </value>
        public bool LeftButtonPressed
        {
            get => _leftButtonPressed;
            set
            {
                if (_leftButtonPressed != value)
                {
                    StateChanged = true;
                }

                _leftButtonPressed = value;
            }
        }

        /// <summary>
        ///     Resets the state.
        /// </summary>
        public void ResetState()
        {
            StateChanged = false;
        }
    }
}