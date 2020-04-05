using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace PanelsApp.Forms
{
    public partial class GroupBoxForm
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
                Text = "GroupBox is a control that displays a frame around a control with an optional caption. It has only one child, but you can used any components for macking rich user interfaces",
            };
            grid.AddContent(lb1, 0, 0, 2);

            targetPanel = new GroupBox()
            {
                Background = Color.DarkMagenta,
                Text = "Header",
            };

            var contentPanel = new Panel
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            contentPanel.AddContent(new Label("Content")
            {
                Background = Color.Magenta,
                Foreground = Color.White,
            });
            targetPanel.AddContent(contentPanel);
            grid.AddContent(targetPanel, 0, 1);

            descriptionLabel = new Label("Let's check how to work TextAlignment, BorderThickness, BorderColor and BorderChars properties")
            {
                Margin = new Thickness(1, 0, 0, 0),
                WordWrap = true,
            };
            grid.AddContent(descriptionLabel, 1, 1);
        }

        public Panel panel1;
        private GroupBox targetPanel;
        private Label descriptionLabel;
    }
}
