using ClForms.Common;
using ClForms.Elements;
using ClForms.Themes;

namespace PanelsApp.Forms
{
    public partial class WelcomeForm
    {
        private void InitializeComponent()
        {
            panel1 = new Panel()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            stackPanel1 = new StackPanel(Orientation.Vertical)
            {
                Parent = panel1,
                Width = 60,
            };
            var gb = new GroupBox()
            {
                BorderThickness = new Thickness(1),
                BorderColor = Color.Cyan,
                Margin = new Thickness(0, 0, 0, 1),
            };
            gb.AddContent(new Label("Welcome to command-line interface forms!!!")
            {
                Foreground = Color.Cyan,
                TextAlignment = TextAlignment.Center,
            });
            stackPanel1.AddContent(gb);
            stackPanel1.AddContent(new Label("This App show you how to use properties and events of components in the list below:")
            {
                TextAlignment = TextAlignment.Center,
                WordWrap = true,
            });
            stackPanel1.AddContent(new Label("Panel")
            {
                TextAlignment = TextAlignment.Center,
                Foreground = Color.Green,
            });
            stackPanel1.AddContent(new Label("GroupBox")
            {
                TextAlignment = TextAlignment.Center,
                Foreground = Color.Green,
            });
            stackPanel1.AddContent(new Label("StackPanel")
            {
                TextAlignment = TextAlignment.Center,
                Foreground = Color.Green,
            });
            stackPanel1.AddContent(new Label("DockPanel")
            {
                TextAlignment = TextAlignment.Center,
                Foreground = Color.Green,
            });
            stackPanel1.AddContent(new Label("If you want to continue press [space bar]. For close this window and exit from app press [Ctrl+F4]")
            {
                TextAlignment = TextAlignment.Center,
                WordWrap = true,
            });
        }

        public Panel panel1;
        private StackPanel stackPanel1;
    }
}
