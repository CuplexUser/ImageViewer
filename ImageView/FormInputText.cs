using System;
using System.Windows.Forms;

namespace ImageViewer
{
    public partial class FormInputText : Form
    {
        public FormInputText()
        {
            InitializeComponent();
        }

        public void InitFormData(string name, string question, string textBoxText, string groupBoxText,
            Func<string> validationFunc)
        {
        }


        private void FormInputText_Load(object sender, EventArgs e)
        {
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}