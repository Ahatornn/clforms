using ClForms.Elements;

namespace ButtonsApp.Forms
{
    public partial class RadioButtonForm: Window
    {
        public RadioButtonForm()
        {
            InitializeComponent();
        }

        private void CheckedChanged(object sender, ClForms.Common.EventArgs.PropertyChangedEventArgs<bool> e)
        {
            if (e.NewValue)
            {
                var radioButton = sender as RadioButton;
                var groupBox = radioButton.Parent.Parent as GroupBox;
                propertyChangedLabel.Text = $"You checked {radioButton.Text} in {groupBox.Text}";
            }
        }
    }
}
