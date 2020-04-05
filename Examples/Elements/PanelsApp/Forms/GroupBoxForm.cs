using System;
using ClForms.Common;
using ClForms.Elements;
using ClForms.Elements.Menu;
using ClForms.Themes;

namespace PanelsApp.Forms
{
    public partial class GroupBoxForm: Window
    {
        private readonly MenuItem mainMenuPropertyItem;

        public GroupBoxForm(MenuItem mainMenuPropertyItem)
        {
            this.mainMenuPropertyItem = mainMenuPropertyItem;
            InitializeComponent();
            PrepareMenuItems();
        }

        private void PrepareMenuItems()
        {
            mainMenuPropertyItem.Items.Clear();
            var taMenuItem = new MenuItem("TextAlignment");
            foreach (var item in Enum.GetValues(typeof(TextAlignment)))
            {
                var menuItem = new MenuItem(item.ToString()) { Tag = item, };
                menuItem.OnClick += TextAlignmentClick;
                taMenuItem.Items.Add(menuItem);
            }
            mainMenuPropertyItem.Items.Add(taMenuItem);

            var btMenuItem = new MenuItem("BorderThickness");
            var btMenuItemValue1 = new MenuItem("Thickness(1) default")
            {
                Tag = new Thickness(1)
            };
            btMenuItemValue1.OnClick += ThicknessClick;
            btMenuItem.Items.Add(btMenuItemValue1);
            var btMenuItemValue2 = new MenuItem("Thickness(1, 2)")
            {
                Tag = new Thickness(1, 2)
            };
            btMenuItemValue2.OnClick += ThicknessClick;
            btMenuItem.Items.Add(btMenuItemValue2);
            var btMenuItemValue3 = new MenuItem("Thickness(1, 2, 3, 4)")
            {
                Tag = new Thickness(1, 2, 3, 4)
            };
            btMenuItemValue3.OnClick += ThicknessClick;
            btMenuItem.Items.Add(btMenuItemValue3);
            mainMenuPropertyItem.Items.Add(btMenuItem);

            var bcMenuItem = new MenuItem("BorderColor");
            foreach (var item in Enum.GetValues(typeof(Color)))
            {
                if ((Color) item == Color.NotSet)
                {
                    continue;
                }
                var menuItem = new MenuItem(item.ToString()) { Tag = item, };
                menuItem.OnClick += BorderColorClick;
                bcMenuItem.Items.Add(menuItem);
            }
            mainMenuPropertyItem.Items.Add(bcMenuItem);

            var brdchMenuItem = new MenuItem("BorderChars");
            var brdchMenuItemValue1 = new MenuItem("┌─┒ ┕━┛")
            {
                Tag = new BorderChars('┌', '─', '┒', '│', '┃', '┕', '━', '┛'),
            };
            brdchMenuItemValue1.OnClick += BorderCharsClick;
            brdchMenuItem.Items.Add(brdchMenuItemValue1);
            var brdchMenuItemValue2 = new MenuItem("╔═╗ ╚═╝")
            {
                Tag = new BorderChars('╔', '═', '╗', '║', '║', '╚', '═', '╝'),
            };
            brdchMenuItemValue2.OnClick += BorderCharsClick;
            brdchMenuItem.Items.Add(brdchMenuItemValue2);
            var brdchMenuItemValue3 = new MenuItem("┌─┐ └─┘")
            {
                Tag = new BorderChars('┌', '─', '┐', '│', '│', '└', '─', '┘'),
            };
            brdchMenuItemValue3.OnClick += BorderCharsClick;
            brdchMenuItem.Items.Add(brdchMenuItemValue3);
            mainMenuPropertyItem.Items.Add(brdchMenuItem);
        }

        private void BorderCharsClick(object sender, EventArgs e)
        {
            targetPanel.BorderChars = (BorderChars) (sender as MenuItem).Tag;
            descriptionLabel.Text = "BorderChars gets or sets the BorderChars that draws the border characters";
        }

        private void BorderColorClick(object sender, EventArgs e)
        {
            targetPanel.BorderColor = (Color) (sender as MenuItem).Tag;
            descriptionLabel.Text = "BorderColor gets or sets the Color that draws the border color";
        }

        private void ThicknessClick(object sender, EventArgs e)
        {
            targetPanel.BorderThickness = (Thickness) (sender as MenuItem).Tag;
            descriptionLabel.Text = "BorderThickness gets or sets the relative frame Thickness of a GroupBox";
        }

        private void TextAlignmentClick(object sender, EventArgs e)
        {
            targetPanel.TextAlignment = (TextAlignment) (sender as MenuItem).Tag;
            descriptionLabel.Text = "TextAlignment gets or sets the horizontal alignment of the text associated with this control";
        }
    }
}
