using System;
using System.Windows.Forms;
using ImageViewer.Events;

namespace ImageViewer
{
    public partial class FormSetSlideshowInterval : Form
    {
        private int _timerInterval;

        public FormSetSlideshowInterval(int timerInterval)
        {
            _timerInterval = timerInterval;
            InitializeComponent();
        }

        private void FormSetSlideshowInterval_Load(object sender, EventArgs e)
        {
            numericUDInterval.Text = _timerInterval.ToString();
        }

        public event IntervalChangedDeligate OnIntervalChanged;

        private void btnOk_Click(object sender, EventArgs e)
        {
            _timerInterval = Convert.ToInt32(numericUDInterval.Value);
            OnIntervalChanged?.Invoke(this, new IntervalEventArgs(_timerInterval));
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtInterval_Validated(object sender, EventArgs e)
        {
            btnOk.Focus();
        }
    }
}