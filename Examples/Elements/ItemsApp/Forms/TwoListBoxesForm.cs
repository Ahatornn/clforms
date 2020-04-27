using System;
using System.Globalization;
using ClForms.Elements;
using ClForms.Elements.Menu;

namespace ItemsApp.Forms
{
    public partial class TwoListBoxesForm: Window
    {
        private readonly MenuItem mainMenuPropertyItem;

        public TwoListBoxesForm(MenuItem mainMenuPropertyItem)
        {
            this.mainMenuPropertyItem = mainMenuPropertyItem;
            InitializeComponent();
            PrepareMenuItems();
            RewindMenuClick(this, EventArgs.Empty);
        }

        private void PrepareMenuItems()
        {
            mainMenuPropertyItem.Items.Clear();
            var rewindMenuItem = new MenuItem("Rewind");
            rewindMenuItem.OnClick += RewindMenuClick;
            mainMenuPropertyItem.Items.Add(rewindMenuItem);
        }

        private void RewindMenuClick(object sender, System.EventArgs e)
        {
            leftListBox.BeginUpdate();
            leftListBox.Items.Clear();
            var dtfInfo = new DateTimeFormatInfo();
            foreach (var dayName in dtfInfo.DayNames)
            {
                leftListBox.Items.Add(dayName);
            }
            leftListBox.SelectedIndex = 0;
            leftListBox.EndUpdate();

            rightListBox.BeginUpdate();
            rightListBox.Items.Clear();
            rightListBox.SelectedIndex = -1;
            rightListBox.EndUpdate();

            UpdateOtherButtons();
        }

        private void ListBoxSelectedIndexChanged(object sender, ClForms.Common.EventArgs.PropertyChangedEventArgs<int> e)
        {
            if (sender is ListBox<string> targetListBox)
            {
                if(targetListBox == leftListBox)
                {
                    leftToRightButton.IsDisabled = targetListBox.SelectedIndex == -1;
                }
                else
                {
                    rightToLeftButton.IsDisabled = targetListBox.SelectedIndex == -1;
                }
            }
        }

        private void LeftToRightButtonClick(object sender, System.EventArgs e)
        {
            var oldSelectedIndex = leftListBox.SelectedIndex;
            rightListBox.Items.Add(leftListBox.Items[oldSelectedIndex]);
            leftListBox.Items.RemoveAt(oldSelectedIndex);
            leftListBox.SelectedIndex = Math.Max(Math.Min(oldSelectedIndex, leftListBox.Items.Count - 1), -1);
            UpdateOtherButtons();
        }

        private void LeftAllToRightButtonClick(object sender, System.EventArgs e)
        {
            foreach (var item in leftListBox.Items)
            {
                rightListBox.Items.Add(item);
            }
            leftListBox.Items.Clear();
            UpdateOtherButtons();
        }

        private void RightToLeftButtonClick(object sender, System.EventArgs e)
        {
            var oldSelectedIndex = rightListBox.SelectedIndex;
            leftListBox.Items.Add(rightListBox.Items[oldSelectedIndex]);
            rightListBox.Items.RemoveAt(oldSelectedIndex);
            rightListBox.SelectedIndex = Math.Max(Math.Min(oldSelectedIndex, rightListBox.Items.Count - 1), -1);
            UpdateOtherButtons();
        }

        private void RightAllToLeftButtonClick(object sender, System.EventArgs e)
        {
            foreach (var item in rightListBox.Items)
            {
                leftListBox.Items.Add(item);
            }
            rightListBox.Items.Clear();
            UpdateOtherButtons();
        }

        private void UpdateOtherButtons()
        {
            leftAllToRightButton.IsDisabled = leftListBox.Items.Count == 0;
            rightAllToLeftButton.IsDisabled = rightListBox.Items.Count == 0;
        }
    }
}
