using System;
using System.Windows.Forms;

namespace ImageViewer
{
    public partial class FormImageDetails : Form
    {
        private readonly string _filename;
        public FormImageDetails(string filename)
        {
            _filename = filename;
            InitializeComponent();
        }

        private void FormImageDetails_Load(object sender, EventArgs e)
        {
            ImgInfoGroupBox.Text = $"Image information about {_filename}";

        }
    }
}
