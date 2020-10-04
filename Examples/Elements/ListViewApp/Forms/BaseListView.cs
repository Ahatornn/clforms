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

        /// <summary>
        /// Initialize a new instance <see cref="BaseListView"/>
        /// </summary>
        public BaseListView(MenuItem mainMenuPropertyItem)
        {
            this.mainMenuPropertyItem = mainMenuPropertyItem;
            InitializeComponent();
            PrepareMenuItems();
        }

        private void PrepareMenuItems() => mainMenuPropertyItem.Items.Clear();
    }
}
