using ClForms.Common;
using ClForms.Elements;
using ClForms.Themes;

namespace MazeGame.Forms
{
    public partial class MapDiscoverWindow
    {
        private void InitializeComponent()
        {
            panel1 = new Panel()
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                AutoSize = false,
            };

            tilePanel = new TilePanel()
            {
                Width = 45,
                Height = 15,
                Background = Color.Black,
                Foreground = Color.Magenta,
                Tile = '░',
            };
            tilePanel.OnPaint += CanvasPaint;
            panel1.AddContent(tilePanel);

            AddContent(panel1);
        }

        public Panel panel1;
        private TilePanel tilePanel;
    }
}
