using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ImageViewer.Models;

namespace ImageViewer.Delegates
{
    public delegate void UpdatePicBoxListEventHandler(object sender, UpdatePicBoxEventArgs eventArgs);

    public class UpdatePicBoxEventArgs : EventArgs
    {
        public PictureBox PictureBoxModel { get; }

        public UpdatePicBoxEventArgs(PictureBox model)
        {
            PictureBoxModel = model;
        }

        public UpdatePicBoxEventArgs()
        {

        }

    }
}