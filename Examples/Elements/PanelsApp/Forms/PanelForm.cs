using System;
using ClForms.Common;
using ClForms.Elements;
using ClForms.Elements.Menu;

namespace PanelsApp.Forms
{
    public partial class PanelForm: Window
    {
        private readonly MenuItem mainMenuPropertyItem;

        public PanelForm(MenuItem mainMenuPropertyItem)
        {
            this.mainMenuPropertyItem = mainMenuPropertyItem;
            InitializeComponent();
            PrepareMenuItems();
        }

        private void PrepareMenuItems()
        {
            mainMenuPropertyItem.Items.Clear();
            var vcaMenuItem = new MenuItem("VerticalContentAlignment");
            foreach (var item in Enum.GetValues(typeof(VerticalAlignment)))
            {
                var menuItem = new MenuItem(item.ToString()) {Tag = item,};
                menuItem.OnClick += VerticalAlignmentClick;
                vcaMenuItem.Items.Add(menuItem);
            }
            mainMenuPropertyItem.Items.Add(vcaMenuItem);

            var hcaMenuItem = new MenuItem("HorizontalContentAlignment");
            foreach (var item in Enum.GetValues(typeof(HorizontalAlignment)))
            {
                var menuItem = new MenuItem(item.ToString()) { Tag = item, };
                menuItem.OnClick += HorizontalAlignmentClick;
                hcaMenuItem.Items.Add(menuItem);
            }
            mainMenuPropertyItem.Items.Add(hcaMenuItem);
        }

        private void VerticalAlignmentClick(object sender, System.EventArgs e)
        {
            targetPanel.VerticalContentAlignment = (VerticalAlignment) (sender as MenuItem).Tag;
            descriptionLabel.Text = "VerticalContentAlignment gets or sets the vertical alignment of the control's content";
        }

        private void HorizontalAlignmentClick(object sender, System.EventArgs e)
        {
            targetPanel.HorizontalContentAlignment = (HorizontalAlignment) (sender as MenuItem).Tag;
            descriptionLabel.Text = "HorizontalContentAlignment gets or sets the horizontal alignment of the control's content";
        }
    }
}
