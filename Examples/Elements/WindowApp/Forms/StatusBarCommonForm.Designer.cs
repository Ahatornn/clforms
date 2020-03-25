using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace WindowApp.Forms
{
    internal partial class StatusBarCommonForm
    {
        private void InitializeComponent()
        {
            panel1 = new Panel
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            var grid = new Grid
            {
                Parent = panel1,
                Width = 70,
                Margin = new Thickness(0, 2),
            };
            grid.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 50));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 50));

            var lb1 = new Label
            {
                WordWrap = true,
                Text = "let's see an example how to use StatusBar in your application. Look at the bottom of the window. There are panel with button and label.",
                Margin = new Thickness(0, 0, 0, 1),
            };

            grid.AddContent(lb1, 0, 0, 2);

            var sp = new StackPanel(Orientation.Vertical);
            sp.AddContent(new Label
            {
                Foreground = Color.DarkBlue,
                Text = "var statusBar1 = new StatusBar();",
            });
            sp.AddContent(new Label
            {
                Foreground = Color.DarkBlue,
                Text = "statusBar1.AddHelpButton();",
            });
            sp.AddContent(new Label
            {
                Foreground = Color.DarkBlue,
                Text = "this.StatusBar = statusBar1;",
            });

            grid.AddContent(sp, 0, 1);

            var lb2 = new Label
            {
                WordWrap = true,
                Text = "First you should to create new instance of StatusBar. Then call \"AddHelpButton\" method for append help button to StatusBar. Finally set \"StatusBar\" property of main window.",
            };
            grid.AddContent(lb2, 1, 1);

            var lb3 = new Label
            {
                WordWrap = true,
                Text = "Let's see now what we prepared for you next. Press [space bar] button for continue",
                Margin = new Thickness(0, 1, 0, 1),
            };

            grid.AddContent(lb3, 0, 2, 2);

            AddContent(panel1);
        }

        public Panel panel1;
    }
}
