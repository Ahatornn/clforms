using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace PanelsApp.Forms
{
    public partial class StackPanelForm
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
                Margin = new Thickness(0, 1),
            };
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 2));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 60));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 40));

            var lb1 = new Label
            {
                WordWrap = true,
                Text = "StackPanel arranges child elements into a single line that can be oriented horizontally or vertically",
            };
            grid.AddContent(lb1, 0, 0, 2);

            targetPanel = new StackPanel(Orientation.Horizontal)
            {
                Background = Color.DarkMagenta,
                AutoSize = false,
            };
            targetPanel.AddContent(new Label("Content1")
            {
                Background = Color.Magenta,
                Foreground = Color.White,
                Margin = new Thickness(1),
            });
            targetPanel.AddContent(new Label("Content2")
            {
                Background = Color.Magenta,
                Foreground = Color.White,
                Margin = new Thickness(1),
            });
            grid.AddContent(targetPanel, 0, 1);

            descriptionLabel = new Label("Let's check how to work Orientation properties")
            {
                Margin = new Thickness(1, 0, 0, 0),
                WordWrap = true,
            };
            grid.AddContent(descriptionLabel, 1, 1);
        }

        public Panel panel1;
        private StackPanel targetPanel;
        private Label descriptionLabel;
    }
}
