using System;
using System.Linq;
using System.Text;
using ClForms.Common;
using ClForms.Elements;
using ClForms.Elements.Menu;
using ClForms.Themes;

namespace GroupsApp.Forms
{
    /// <summary>
    /// The main Window of App
    /// </summary>
    public partial class CheckBoxGroupForm: Window
    {
        private readonly MenuItem mainMenuPropertyItem;

        /// <summary>
        /// Initialize a new instance <see cref="CheckBoxGroupForm"/>
        /// </summary>
        public CheckBoxGroupForm(MenuItem mainMenuPropertyItem)
        {
            this.mainMenuPropertyItem = mainMenuPropertyItem;
            InitializeComponent();
            targetPanel.Columns = 3;
            targetPanel.Items.Add("January");
            targetPanel.Items.Add("February");
            targetPanel.Items.Add("March");
            targetPanel.Items.Add("April", true);
            targetPanel.Items.Add("May");
            targetPanel.Items.Add("June");
            targetPanel.Items.Add("July");
            targetPanel.Items.Add("August");
            targetPanel.Items.Add("September");
            targetPanel.Items.Add("October");
            targetPanel.Items.Add("November");
            targetPanel.Items.Add("December");
            targetPanel.OnSelectedCheckedChanged += PanelSelectedCheckedChanged;
            PrepareMenuItems();
        }

        private void PanelSelectedCheckedChanged(object sender, ClForms.Common.EventArgs.SelectedItemCheckedEventArgs e)
        {
            descriptionLabel.Text = "Listen OnSelectedCheckedChanged event for detect with item user checked";
            var text = "No one item was checked";
            if (targetPanel.SelectedIndexes.Any())
            {
                var sb = new StringBuilder();
                foreach (var index in targetPanel.SelectedIndexes)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }

                    sb.Append(targetPanel.Items[index].Text);
                }

                text = $"{sb} {(targetPanel.SelectedIndexes.Count() > 1 ? "were" : "was")} checked";
            }

            MessageBox.Show(text);
        }

        private void PrepareMenuItems()
        {
            #region Columns

            mainMenuPropertyItem.Items.Clear();
            var colMenuItem = new MenuItem("Columns");
            for (var i = 3; i > 0; i--)
            {
                var menuItem = new MenuItem($"{i} column{(i == 1 ? string.Empty : "s")}") { Tag = i, };
                menuItem.OnClick += ColumnItemClick;
                colMenuItem.Items.Add(menuItem);
            }
            mainMenuPropertyItem.Items.Add(colMenuItem);

            #endregion
            #region TextAlignment

            var taMenuItem = new MenuItem("TextAlignment");
            foreach (var item in Enum.GetValues(typeof(TextAlignment)))
            {
                var menuItem = new MenuItem(item.ToString()) { Tag = item, };
                menuItem.OnClick += TextAlignmentClick;
                taMenuItem.Items.Add(menuItem);
            }
            mainMenuPropertyItem.Items.Add(taMenuItem);

            #endregion
            #region BorderColor

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

            #endregion
            #region BorderChars

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

            #endregion
            mainMenuPropertyItem.Items.Add(new SeparatorMenuItem());
            #region AutoSize

            var autoSizeMenuItem = new MenuItem("AutoSize") { Checked = false, };
            autoSizeMenuItem.OnClick += AutoSizeMenuItemClick;
            mainMenuPropertyItem.Items.Add(autoSizeMenuItem);

            #endregion
            mainMenuPropertyItem.Items.Add(new SeparatorMenuItem());
            #region CheckAll

            var checkAllMenuItem = new MenuItem("CheckAll") { Checked = false, };
            checkAllMenuItem.OnClick += CheckAllMenuItemClick;
            ;
            mainMenuPropertyItem.Items.Add(checkAllMenuItem);

            #endregion
            #region UncheckAll

            var uncheckAllMenuItem = new MenuItem("UncheckAll") { Checked = false, };
            uncheckAllMenuItem.OnClick += UncheckAllMenuItemClick;
            ;
            mainMenuPropertyItem.Items.Add(uncheckAllMenuItem);

            #endregion
        }

        private void UncheckAllMenuItemClick(object sender, EventArgs e)
        {
            targetPanel.UncheckAll(true);
            descriptionLabel.Text = "UncheckAll method allowed uncheck all the items in the CheckBoxGroup. If set ExcludeDisabled it will not uncheck disabled items";
        }

        private void CheckAllMenuItemClick(object sender, EventArgs e)
        {
            targetPanel.CheckAll(true);
            descriptionLabel.Text = "CheckAll method allowed checks all the items in the CheckBoxGroup. If set ExcludeDisabled it will not check disabled items";
        }

        private void AutoSizeMenuItemClick(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            menuItem.Checked = !menuItem.Checked;
            targetPanel.AutoSize = menuItem.Checked;
        }

        private void ColumnItemClick(object sender, System.EventArgs e)
        {
            targetPanel.Columns = (int) (sender as MenuItem).Tag;
            descriptionLabel.Text = "Columns gets or sets the number of columns in the switch group. If it has more items than allowed they will disappear";
        }

        private void TextAlignmentClick(object sender, EventArgs e)
        {
            targetPanel.TextAlignment = (TextAlignment) (sender as MenuItem).Tag;
            descriptionLabel.Text = "TextAlignment gets or sets the horizontal alignment of the text associated with this control";
        }

        private void BorderColorClick(object sender, EventArgs e)
        {
            targetPanel.BorderColor = (Color) (sender as MenuItem).Tag;
            descriptionLabel.Text = "BorderColor gets or sets the Color that draws the border color";
        }

        private void BorderCharsClick(object sender, EventArgs e)
        {
            targetPanel.BorderChars = (BorderChars) (sender as MenuItem).Tag;
            descriptionLabel.Text = "BorderChars gets or sets the BorderChars that draws the border characters";
        }
    }
}
