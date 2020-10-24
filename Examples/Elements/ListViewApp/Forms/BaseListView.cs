using ClForms.Elements;
using ClForms.Elements.Menu;

namespace ListViewApp.Forms
{
    /// <summary>
    /// The main Window of App
    /// </summary>
    public partial class BaseListView: Window
    {
        private readonly MenuItem mainMenuPropertyItem;
        private bool isSelectedViewMode;

        /// <summary>
        /// Initialize a new instance <see cref="BaseListView"/>
        /// </summary>
        public BaseListView(MenuItem mainMenuPropertyItem)
        {
            this.mainMenuPropertyItem = mainMenuPropertyItem;
            InitializeComponent();
            PrepareMenuItems();
        }

        private void PrepareMenuItems()
        {
            mainMenuPropertyItem.Items.Clear();

            var gridLineMenuItem = new MenuItem("ShowGridLine", null, @checked: true);
            gridLineMenuItem.OnClick += GridLineMenuItemClick;
            mainMenuPropertyItem.Items.Add(gridLineMenuItem);

            var summaryMenuItem = new MenuItem("ShowSummary", null, @checked: true);
            summaryMenuItem.OnClick += SummaryMenuItemClick;
            mainMenuPropertyItem.Items.Add(summaryMenuItem);

            mainMenuPropertyItem.Items.Add(new SeparatorMenuItem());

            var itemsMenuItem = new MenuItem("Items");
            itemsMenuItem.OnClick += ItemsMenuItemClick;
            mainMenuPropertyItem.Items.Add(itemsMenuItem);

            var selectedIndexMenuItem = new MenuItem("SelectedIndex");
            selectedIndexMenuItem.OnClick += SelectedIndexMenuItemClick;
            mainMenuPropertyItem.Items.Add(selectedIndexMenuItem);
        }

        private void SelectedIndexMenuItemClick(object sender, System.EventArgs e)
        {
            isSelectedViewMode = true;
            descriptionLabel.Text = "FirstVisibleItemIndex has an index of first visible item, " +
                                    "SelectedIndex has an index of the selected item in the control. " +
                                    "Also use VisibleItems property for get all visible items.";
            codeLabel.Lines = new[]
            {
                $"FirstVisibleItemIndex: {targetListView.FirstVisibleItemIndex}",
                $"SelectedIndex: {targetListView.SelectedIndex}",
            };
        }

        private void ListViewSelectedIndexChanged(object sender, ClForms.Common.EventArgs.PropertyChangedEventArgs<int> e)
        {
            if (isSelectedViewMode)
            {
                codeLabel.Lines = new[]
                {
                    $"FirstVisibleItemIndex: {targetListView.FirstVisibleItemIndex}",
                    $"SelectedIndex: {e.NewValue}",
                };
            }
        }

        private void ItemsMenuItemClick(object sender, System.EventArgs e)
        {
            isSelectedViewMode = false;
            descriptionLabel.Text = "Items has a collection containing all items in the control";
            codeLabel.Lines = new[]
            {
                "…",
                "var items = LoadData(…);",
                "…",
                "…",
                "targetListView.Items.AddRange(items);",
                "…",
            };
        }

        private void SummaryMenuItemClick(object sender, System.EventArgs e)
        {
            isSelectedViewMode = false;
            if (sender is MenuItem menuItem)
            {
                descriptionLabel.Text = "ShowSummary has a value indicating whether show summary area for SummaryText property";
                var value = !menuItem.Checked;
                codeLabel.Lines = new []
                {
                    "…",
                    $"targetListView.ShowSummary = {value.ToString().ToLower()};",
                    "…",
                };
                menuItem.Checked = value;
                targetListView.ShowSummary = value;
            }
        }

        private void GridLineMenuItemClick(object sender, System.EventArgs e)
        {
            isSelectedViewMode = false;
            if (sender is MenuItem menuItem)
            {
                descriptionLabel.Text = "ShowGridLine has a value indicating whether grid lines appear between the columns containing the items in the control";
                var value = !menuItem.Checked;
                codeLabel.Lines = new[]
                {
                    "…",
                    $"targetListView.ShowGridLine = {value.ToString().ToLower()};",
                    "…",
                };
                menuItem.Checked = value;
                targetListView.ShowGridLine = value;
            }
        }
    }
}
