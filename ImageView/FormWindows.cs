using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ImageViewer.Events;
using ImageViewer.Interfaces;
using ImageViewer.Services;

namespace ImageViewer
{
    public partial class FormWindows : Form, IObserver<ImageViewFormInfoBase>
    {
        private readonly List<IDisposable> _formDisposables;
        private readonly Dictionary<Form, ImageWindowListItem> _imageWindowListItems;
        private Form _lastActiveImageViewForm;
        private int _winCnt;
        private readonly ApplicationSettingsService _applicationSettingsService;

        public FormWindows(ApplicationSettingsService applicationSettingsService)
        {
            _applicationSettingsService = applicationSettingsService;
            InitializeComponent();
            _formDisposables = new List<IDisposable>();
            _imageWindowListItems = new Dictionary<Form, ImageWindowListItem>();
        }

        public void OnNext(ImageViewFormInfoBase value)
        {
            if (value is ImageViewFormImageInfo)
            {
                var frmImageInfo = value as ImageViewFormImageInfo;
                if (!_imageWindowListItems.ContainsKey(frmImageInfo.FormReference))
                    _imageWindowListItems.Add(frmImageInfo.FormReference,
                        new ImageWindowListItem
                        {
                            Name = frmImageInfo.CurrentImageFileName,
                            Value = frmImageInfo.ImagesViewed.ToString()
                        });

                if (frmImageInfo.FormIsClosing)
                {
                    _imageWindowListItems.Remove(frmImageInfo.FormReference);
                    RenderWindowList();

                    return;
                }

                ImageWindowListItem imageWindowListItem = _imageWindowListItems[frmImageInfo.FormReference];
                imageWindowListItem.Name = frmImageInfo.CurrentImageFileName ?? "Empty Window";
                imageWindowListItem.Value = _winCnt++.ToString();
                imageWindowListItem.WindowRef = frmImageInfo.FormReference;

                RenderWindowList();
                _lastActiveImageViewForm = value.FormReference;
            }
            else
            {
                if (!(value is ImageViewFormInfo frmStateInfo) || frmStateInfo.FormReference != _lastActiveImageViewForm) return;
                if (frmStateInfo.FormHasFocus)
                    _lastActiveImageViewForm = null;
                else if (frmStateInfo.FormHasFocus)
                    _lastActiveImageViewForm = frmStateInfo.FormReference;
            }
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
            RenderWindowList();
        }

        private void FormWindows_Load(object sender, EventArgs e)
        {
            chkShowInTaskBar.Checked = _applicationSettingsService.Settings.ShowImageViewFormsInTaskBar;
        }

        public void SubscribeToList(List<FormImageView> providers)
        {
            foreach (FormImageView provider in providers)
                _formDisposables.Add(provider.Subscribe(this));
        }

        public void Subscribe(FormImageView provider)
        {
            _formDisposables.Add(provider.Subscribe(this));
        }

        private void Unsubscribe()
        {
            foreach (IDisposable observableForm in _formDisposables)
                observableForm.Dispose();
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            foreach (object form in listBoxActiveWindows.SelectedItems)
            {
                ((ImageWindowListItem) form).WindowRef.Show();
                ((ImageWindowListItem) form).WindowRef.Focus();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            var closeQueue = new Queue<ImageWindowListItem>();
            foreach (object form in listBoxActiveWindows.SelectedItems)
            {
                var imageWindowListItem = form as ImageWindowListItem;
                closeQueue.Enqueue(imageWindowListItem);
            }

            while (closeQueue.Count > 0)
                closeQueue.Dequeue().WindowRef.Close();
        }

        private void RenderWindowList()
        {
            listBoxActiveWindows.Items.Clear();
            foreach (ImageWindowListItem listItem in _imageWindowListItems.Values)
                listBoxActiveWindows.Items.Add(listItem);
        }

        private void FormWindows_FormClosed(object sender, FormClosedEventArgs e)
        {
            Unsubscribe();
        }

        private void btnCascade_Click(object sender, EventArgs e)
        {
            int x = 10;
            int y = 8;
            foreach (object form in listBoxActiveWindows.SelectedItems)
            {
                if (form is ImageWindowListItem imageWindowListItem)
                {
                    imageWindowListItem.WindowRef.Location = new Point(x, y);
                    imageWindowListItem.WindowRef.Size = new Size(400, 300);
                    imageWindowListItem.WindowRef.Focus();
                    x += 8;
                    y += 30;

                    var imageViewFormWindow = imageWindowListItem.WindowRef as IMageViewFormWindow;
                    imageViewFormWindow?.ResetZoomAndRepaint();
                }
            }
        }

        private void btnSideBySide_Click(object sender, EventArgs e)
        {
            var screenList = new List<Screen>(Screen.AllScreens).OrderBy(left => left.Bounds.Left).ToList();
            Screen myScreen = screenList.First();

            const int maxWindowsHorizontal = 3;
            const int maxWindowsVertical = 2;
            int screenWidthOffset = _applicationSettingsService.Settings.ScreenWidthOffset;
            var windowPosision = new Point(myScreen.WorkingArea.Left - 5, myScreen.WorkingArea.Top);

            foreach (object form in listBoxActiveWindows.SelectedItems)
            {
                if (form is ImageWindowListItem imageWindowListItem)
                {
                    imageWindowListItem.WindowRef.DesktopBounds = GetWindowSizeFromScreen(myScreen, maxWindowsHorizontal,
                        maxWindowsVertical, screenWidthOffset, windowPosision);
                    imageWindowListItem.WindowRef.StartPosition = FormStartPosition.Manual;

                    var imageViewWindow = imageWindowListItem.WindowRef as IMageViewFormWindow;
                    imageViewWindow?.ResetZoomAndRepaint();
                    imageWindowListItem.WindowRef.Focus();


                    windowPosision.X += imageWindowListItem.WindowRef.DesktopBounds.Width - 10;
                    if (windowPosision.X + imageWindowListItem.WindowRef.DesktopBounds.Width - 2 >
                        myScreen.WorkingArea.Right)
                    {
                        windowPosision.X = myScreen.WorkingArea.Left - 5;
                        windowPosision.Y += imageWindowListItem.WindowRef.DesktopBounds.Height - 5;
                    }

                    if (windowPosision.Y > myScreen.WorkingArea.Height)
                    {
                        screenList.Remove(myScreen);
                        myScreen = screenList.FirstOrDefault();
                        if (myScreen == null)
                            break;

                        windowPosision = new Point(myScreen.WorkingArea.Left - 5, myScreen.WorkingArea.Top);
                    }
                }
            }
            Focus();
        }

        private Rectangle GetWindowSizeFromScreen(Screen screen, int maxWindowsHorizontal, int maxWindowsVertical,
            int screenWidthOffset, Point windowPosision)
        {
            return new Rectangle(windowPosision,
                new Size((screen.WorkingArea.Width + screenWidthOffset)/maxWindowsHorizontal,
                    screen.WorkingArea.Height/maxWindowsVertical + 5));
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            foreach (object form in listBoxActiveWindows.SelectedItems)
            {
                var imageWindowListItem = form as ImageWindowListItem;
                imageWindowListItem?.WindowRef.Hide();
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            foreach (object form in listBoxActiveWindows.SelectedItems)
            {
                var imageWindowListItem = form as ImageWindowListItem;
                imageWindowListItem?.WindowRef.Show();
            }
        }

        private void chkShowInTaskBar_CheckedChanged(object sender, EventArgs e)
        {
            bool showInTaskbar = chkShowInTaskBar.Checked;
            _applicationSettingsService.Settings.ShowImageViewFormsInTaskBar = showInTaskbar;
            foreach (object form in listBoxActiveWindows.Items)
            {
                if (form is ImageWindowListItem imageWindowListItem)
                    imageWindowListItem.WindowRef.ShowInTaskbar = showInTaskbar;
            }
        }

        public void RestoreFocusOnPreviouslyActiveImageForm()
        {
            if (_lastActiveImageViewForm != null && _lastActiveImageViewForm.Visible)
            {
                _lastActiveImageViewForm.Show();
                _lastActiveImageViewForm.Focus();
            }
        }

        public void RestoreFocusToMainForm()
        {
            _lastActiveImageViewForm = null;
        }

        private class ImageWindowListItem
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public Form WindowRef { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}