using ClForms.Common;
using ClForms.Elements;

namespace ButtonsApp.Forms
{
    public partial class MessageBoxForm: Window
    {
        public MessageBoxForm()
        {
            InitializeComponent();
        }

        private void FullClick(object sender, System.EventArgs e)
            => MessageBox.Show("Warning caption is here", "This is a message box with the specified text, caption, icon and buttons. You can show a text with automatically word wrap property. This dialog returns one of the [Yes, No, Cancel] values", MessageBoxIcon.Warning, MessageBoxButtons.YesNoCancel);

        private void CaptionIconClick(object sender, System.EventArgs e)
            => MessageBox.ShowError("Caption of error message ", "This is a message box for show error message");

        private void CaptionClick(object sender, System.EventArgs e)
            => MessageBox.Show("Caption is here", "This is a simple message box with caption");

        private void SimpleClick(object sender, System.EventArgs e)
            => MessageBox.Show("This is a simple message box");
    }
}
