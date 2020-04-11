using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace PanelsApp.Forms
{
    public partial class TilePanelForm
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
                Text = "TilePanel defines the char tiling element. It hasn't any child components, but you can tiling by char for macking specific user interfaces",
            };
            grid.AddContent(lb1, 0, 0, 2);

            targetPanel = new TilePanel('░')
            {
                Background = Color.DarkMagenta,
                Foreground = Color.Cyan,
            };

            grid.AddContent(targetPanel, 0, 1);

            descriptionLabel = new Label("Let's check how to work Tile property")
            {
                Margin = new Thickness(1, 0, 0, 0),
                WordWrap = true,
            };
            grid.AddContent(descriptionLabel, 1, 1);
        }

        public Panel panel1;
        private TilePanel targetPanel;
        private Label descriptionLabel;
    }
}
