using ClForms.Elements;
using ClForms.Elements.Menu;
using ClForms.Themes;

namespace PanelsApp.Forms
{
    public partial class TilePanelForm: Window
    {
        private readonly MenuItem mainMenuPropertyItem;
        private const string Tiles = "░◘☻♥";

        public TilePanelForm(MenuItem mainMenuPropertyItem)
        {
            this.mainMenuPropertyItem = mainMenuPropertyItem;
            InitializeComponent();
            PrepareMenuItems();
        }

        private void PrepareMenuItems()
        {
            mainMenuPropertyItem.Items.Clear();
            for (var i = 0; i < Tiles.Length; i++)
            {
                var tileMenuItem = new MenuItem($"Tile {Tiles[i]}") {Tag = i};
                tileMenuItem.OnClick += TileClick;
                mainMenuPropertyItem.Items.Add(tileMenuItem);
            }
        }

        private void TileClick(object sender, System.EventArgs e)
        {
            int.TryParse((sender as MenuItem).Tag.ToString(), out var elementIndex);
            targetPanel.Tile = Tiles[elementIndex];
            if (elementIndex == 0)
            {
                targetPanel.Background = Color.DarkMagenta;
                targetPanel.Foreground = Color.Cyan;
            }
            if (elementIndex == 1)
            {
                targetPanel.Background = Color.DarkCyan;
                targetPanel.Foreground = Color.Gray;
            }
            if (elementIndex == 2)
            {
                targetPanel.Background = Color.White;
                targetPanel.Foreground = Color.Black;
            }
            if (elementIndex == 3)
            {
                targetPanel.Background = Color.Black;
                targetPanel.Foreground = Color.Red;
            }
        }
    }
}
