using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace PanelsApp.Forms
{
    public partial class DockPanelForm
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
                Text = "DockPanel defines an area where you can arrange child elements either horizontally or vertically, relative to each other",
            };
            grid.AddContent(lb1, 0, 0, 2);

            targetPanel = new DockPanel()
            {
                Background = Color.DarkMagenta,
            };

            grid.AddContent(targetPanel, 0, 1);

            descriptionLabel = new Label("Let's check how to arrange child elements and what is the LastChildFill property")
            {
                Margin = new Thickness(1, 0, 0, 0),
                WordWrap = true,
            };
            grid.AddContent(descriptionLabel, 1, 1);
        }

        public Panel panel1;
        private DockPanel targetPanel;
        private Label descriptionLabel;
    }
}
