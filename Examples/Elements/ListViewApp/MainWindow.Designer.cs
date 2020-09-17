using ClForms.Common;
using ClForms.Common.Grid;
using ClForms.Elements;
using ClForms.Themes;

namespace ListViewApp
{
    public partial class MainWindow
    {
        /// <summary>
        /// Initialize components of Window as buttons, panels, etc.
        /// </summary>
        private void InitializeComponent()
        {
            WindowState = ControlState.Maximized;
            Title = "Norton commander emulator";
            AutoSize = false;

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition(SizeType.AutoSize));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.AutoSize));
            grid.ColumnDefinitions.Add(new ColumnDefinition(SizeType.AutoSize));

            listView1 = new ListView<DiskItem>
            {
                Background = Color.Blue,
                BorderColor = Color.White,
                Foreground = Color.White,
                TextAlignment = TextAlignment.Center,
            };
            grid.AddContent(listView1);

            listView2 = new ListView<DiskItem>
            {
                Background = Color.Blue,
                BorderColor = Color.White,
                Foreground = Color.White,
                TextAlignment = TextAlignment.Center,
            };
            grid.AddContent(listView2, 1, 0);

            AddContent(grid);
        }

        private ListView<DiskItem> listView1;
        private ListView<DiskItem> listView2;
    }
}
