using ClForms.Elements;

namespace ButtonsApp.Forms
{
    public partial class CheckBoxForm: Window
    {
        public CheckBoxForm()
        {
            InitializeComponent();
        }

        private void CheckedChanged(object sender, ClForms.Common.EventArgs.PropertyChangedEventArgs<bool> e)
        {
            var checkBox = sender as CheckBox;
            propertyChangedLabel.Text = $"{checkBox.Text} was clicked. Checked: {checkBox.Checked}";
        }
    }
}
