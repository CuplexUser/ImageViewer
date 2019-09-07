using System;
using System.Windows.Forms;

namespace ImageViewer.InputForms
{
    public partial class FormInputRow : Form
    {
        private readonly InputFormData _inputFormData;
        public FormInputRow(InputFormData inputFormData)
        {
            _inputFormData = inputFormData;
            InitializeComponent();
        }

        public string UserInputText { get; set; }

        private void FormInputRowData_Load(object sender, EventArgs e)
        {
            Text = _inputFormData.WindowText;
            groupBoxMain.Text = _inputFormData.GroupBoxText;
            labelInput.Text = _inputFormData.LabelText;
        }

        private void FormInputRow_Shown(object sender, EventArgs e)
        {
            txtInput.Focus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                UserInputText = txtInput.Text;
                Close();
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show($"The selected name is invalid. Minimum {_inputFormData.MinimumCharacters} characters and maximum {_inputFormData.MaximumCharacters} characters", "Invalid input", MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool ValidateInput()
        {
            return txtInput.Text.Length >= _inputFormData.MinimumCharacters && txtInput.Text.Length <= _inputFormData.MaximumCharacters;
        }

        public sealed class InputFormData
        {
            public string WindowText { get; set; }
            public string GroupBoxText { get; set; }
            public string LabelText { get; set; }
            public int MinimumCharacters { get; set; }
            public int MaximumCharacters { get; set; }
        }
    }
}