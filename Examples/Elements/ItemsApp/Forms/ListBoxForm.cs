using System;
using System.Linq;
using ClForms.Elements;
using ClForms.Elements.Menu;
using ClForms.Themes;

namespace ItemsApp.Forms
{
    public partial class ListBoxForm: Window
    {
        private readonly MenuItem mainMenuPropertyItem;
        private string[] formatStrings = new []
        {
            "[{0}]",
            "item: {0}",
            "...{0}...",
            " {0} ",
        };

        public ListBoxForm(MenuItem mainMenuPropertyItem)
        {
            this.mainMenuPropertyItem = mainMenuPropertyItem;
            InitializeComponent();
            UpdateMethrics();
            PrepareMenuItems();
        }

        private void PrepareMenuItems()
        {
            mainMenuPropertyItem.Items.Clear();
            var backgroundMenuItem = new MenuItem("SelectedBackground");
            var foregroundMenuItem = new MenuItem("SelectedForeground");

            foreach (var value in Enum.GetValues(typeof(Color))
                .Cast<Color>()
                .Where(x => x != Color.NotSet)
                .OrderBy(x => x.ToString()))
            {
                if (value.ToString().StartsWith("Dark"))
                {
                    var bgClItem = new MenuItem(value.ToString())
                    {
                        Tag = value
                    };
                    bgClItem.OnClick += BackgroundItemClick;
                    backgroundMenuItem.Items.Add(bgClItem);
                }
                else
                {
                    var fgClItem = new MenuItem(value.ToString())
                    {
                        Tag = value
                    };
                    fgClItem.OnClick += ForegroundItemClick;
                    foregroundMenuItem.Items.Add(fgClItem);
                }
            }
            mainMenuPropertyItem.Items.Add(backgroundMenuItem);
            mainMenuPropertyItem.Items.Add(foregroundMenuItem);

            var formatStaingMenuItem = new MenuItem("Format string");
            foreach (var item in formatStrings)
            {
                var fsItem = new MenuItem(item)
                {
                    Tag = item
                };
                fsItem.OnClick += FormatItemClick;
                formatStaingMenuItem.Items.Add(fsItem);
            }
            mainMenuPropertyItem.Items.Add(new SeparatorMenuItem());
            mainMenuPropertyItem.Items.Add(formatStaingMenuItem);
        }

        private void BackgroundItemClick(object sender, EventArgs e)
        {
            if (sender is MenuItem targetMenuItem)
            {
                targetListBox.SelectedBackground = (Color) targetMenuItem.Tag;
            }
        }

        private void ForegroundItemClick(object sender, EventArgs e)
        {
            if (sender is MenuItem targetMenuItem)
            {
                targetListBox.SelectedForeground = (Color) targetMenuItem.Tag;
            }
        }

        private void FormatItemClick(object sender, EventArgs e)
        {
            if (sender is MenuItem targetMenuItem)
            {
                targetListBox.FormatString = targetMenuItem.Tag.ToString();
            }
        }

        private void ListBoxSelectedIndexChanged(object sender, ClForms.Common.EventArgs.PropertyChangedEventArgs<int> e)
        {
            selectedIndexLabel.Text = e.NewValue.ToString();
            removeButton.IsDisabled = e.NewValue == -1;
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            targetListBox.Items.Add(new ItemTest());
            UpdateMethrics();
        }

        private void RemoveButtonClick(object sender, EventArgs e)
        {
            var item = targetListBox.Items[targetListBox.SelectedIndex];
            if(MessageBox.Show($"Do you really want to remove item '{item}'?", ClForms.Common.MessageBoxIcon.Question, ClForms.Common.MessageBoxButtons.YesNo) == ClForms.Common.DialogResult.Yes)
            {
                targetListBox.Items.Remove(item);
                UpdateMethrics();
            }
        }

        private void UpdateMethrics()
        {
            countLabel.Text = targetListBox.Items.Count.ToString();
            selectedIndexLabel.Text = targetListBox.SelectedIndex.ToString();
            formatStringLabel.Text = targetListBox.FormatString;
        }
    }
}
