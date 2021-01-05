using System;
using ClForms.Elements;

namespace WindowApp.Forms
{
    /// <summary>
    /// The main Window of App
    /// </summary>
    public partial class TextBoxForm: Window
    {
        /// <summary>
        /// Initialize a new instance <see cref="TextBoxForm"/>
        /// </summary>
        public TextBoxForm()
        {
            InitializeComponent();
        }

        private void ShowDescriptionByControlEnter(object sender, System.EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                descriptionCodeLabel.Text = textBox.Tag?.ToString();
            }
        }

        private void TextChanged(object sender, ClForms.Common.EventArgs.PropertyChangedEventArgs<string> e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Tag = $"PasswordChar has character used to mask characters of a password in the control. Original text is: '{e.NewValue}'";
                ShowDescriptionByControlEnter(sender, EventArgs.Empty);
            }
        }
    }
}
