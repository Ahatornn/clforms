using System;
using ClForms.Common;
using ClForms.Elements;
using ClForms.Elements.Menu;

namespace PanelsApp.Forms
{
    public partial class StackPanelForm: Window
    {
        private readonly MenuItem mainMenuPropertyItem;

        public StackPanelForm(MenuItem mainMenuPropertyItem)
        {
            this.mainMenuPropertyItem = mainMenuPropertyItem;
            InitializeComponent();
            PrepareMenuItems();
        }

        private void PrepareMenuItems()
        {
            mainMenuPropertyItem.Items.Clear();
            var otMenuItem = new MenuItem("Orientation");
            foreach (var item in Enum.GetValues(typeof(Orientation)))
            {
                var menuItem = new MenuItem(item.ToString()) { Tag = item, };
                menuItem.OnClick += OrientationClick;
                otMenuItem.Items.Add(menuItem);
            }
            mainMenuPropertyItem.Items.Add(otMenuItem);
        }

        private void OrientationClick(object sender, EventArgs e)
        {
            targetPanel.Orientation = (Orientation) (sender as MenuItem).Tag;
            descriptionLabel.Text = "Orientation gets or sets a value that indicates the dimension by which child elements are stacked";
        }
    }
}
