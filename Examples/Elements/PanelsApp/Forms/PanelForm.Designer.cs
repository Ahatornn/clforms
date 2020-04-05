using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace PanelsApp.Forms
{
    public partial class PanelForm
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
            grid.RowDefinitions.Add(new RowDefinition(SizeType.Absolute, 3));
            grid.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));

            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 60));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.Percent, 40));

            var lb1 = new Label
            {
                WordWrap = true,
                Text = "Panel is a simple control using to group collections of controls. It has only one child, but you can used any components for macking rich user interfaces",
            };
            grid.AddContent(lb1, 0, 0, 2);

            targetPanel = new Panel()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Background = Color.DarkMagenta,
            };
            targetPanel.AddContent(new Label("Content")
            {
                Background = Color.Magenta,
                Foreground = Color.White,
            });
            grid.AddContent(targetPanel, 0, 1);

            descriptionLabel = new Label("Let's check how to work VerticalContentAlignment and HorizontalContentAlignment properties")
            {
                Margin = new Thickness(1, 0, 0, 0),
                WordWrap = true,
            };
            grid.AddContent(descriptionLabel, 1, 1);
        }

        public Panel panel1;
        private Panel targetPanel;
        private Label descriptionLabel;
    }
}
