using ClForms.Elements;

namespace ButtonsApp.Forms
{
    public partial class ButtonForm: Window
    {
        public ButtonForm()
        {
            InitializeComponent();
        }

        private void BtnClick(object sender, System.EventArgs e)
        {
            var caption = (sender as Button).Text;
            MessageBox.Show("Button click result", $"It was  clicked on the '{caption}' control");
        }
    }
}
