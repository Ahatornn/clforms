using ClForms.Common;
using ClForms.Elements;

namespace WindowApp
{
    public partial class MainWindow
    {
        private void InitializeComponent()
        {
            Width = 80;
            Height = 20;
            Title = "Main Window";

            panel1 = new Panel()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };

            statusBar1 = new StatusBar();
            statusBar1.AddHelpButton();
            statusBar1.AddLabel("label docked right", Dock.Right);

            AddContent(panel1);
        }

        private Panel panel1;
        private StatusBar statusBar1;
    }
}
