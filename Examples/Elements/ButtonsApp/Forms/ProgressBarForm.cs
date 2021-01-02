using System;
using System.Linq;
using ClForms.Elements;
using ClForms.Elements.Menu;
using ClForms.Themes;

namespace ButtonsApp.Forms
{
    /// <summary>
    /// The main Window of App
    /// </summary>
    public partial class ProgressBarForm: Window
    {
        private readonly MenuItem mainMenuPropertyItem;
        /// <summary>
        /// Initialize a new instance <see cref="ProgressBarForm"/>
        /// </summary>
        public ProgressBarForm(MenuItem mainMenuPropertyItem)
        {
            this.mainMenuPropertyItem = mainMenuPropertyItem;
            InitializeComponent();
            PrepareMenuItems();
        }

        private void PrepareMenuItems()
        {
            mainMenuPropertyItem.Items.Clear();

            var showValueMenuItem = new MenuItem("ShowValue");
            showValueMenuItem.OnClick += ShowValueMenuItemClick;
            mainMenuPropertyItem.Items.Add(showValueMenuItem);

            var valueAsPerCentMenuItem = new MenuItem("ValueAsPerCent");
            valueAsPerCentMenuItem.Checked = true;
            valueAsPerCentMenuItem.OnClick += ValueAsPerCentMenuItemClick;
            mainMenuPropertyItem.Items.Add(valueAsPerCentMenuItem);

            mainMenuPropertyItem.Items.Add(new SeparatorMenuItem());

            var backgroundMenuItem = new MenuItem("Background");
            var selectedMenuItem = new MenuItem("SelectedBackground");

            foreach (var value in Enum.GetValues(typeof(Color))
                .Cast<Color>()
                .Where(x => x != Color.NotSet)
                .OrderBy(x => x.ToString()))
            {
                if (value.ToString().StartsWith("Dark"))
                {
                    var selItem = new MenuItem(value.ToString())
                    {
                        Tag = value
                    };
                    selItem.OnClick += SelectedBackgroundClick;
                    selectedMenuItem.Items.Add(selItem);
                }
                else
                {
                    var bgItem = new MenuItem(value.ToString())
                    {
                        Tag = value
                    };
                    bgItem.OnClick += BackgroundClick;
                    ;
                    backgroundMenuItem.Items.Add(bgItem);
                }
            }
            mainMenuPropertyItem.Items.Add(backgroundMenuItem);
            mainMenuPropertyItem.Items.Add(selectedMenuItem);
        }

        private void BackgroundClick(object sender, EventArgs e)
        {
            if (sender is MenuItem targetMenuItem)
            {
                targetProgressBar.Background = (Color) targetMenuItem.Tag;
            }
        }

        private void SelectedBackgroundClick(object sender, EventArgs e)
        {
            if (sender is MenuItem targetMenuItem)
            {
                targetProgressBar.SelectedBackground = (Color) targetMenuItem.Tag;
            }
        }

        private void ValueAsPerCentMenuItemClick(object sender, EventArgs e)
        {
            if (sender is MenuItem targetMenuItem)
            {
                targetMenuItem.Checked = !targetMenuItem.Checked;
                targetProgressBar.ValueAsPerCent = targetMenuItem.Checked;
                descriptionLabel.Text = "ValueAsPerCent describes the value should be displayed inside the component as a percentage, provided that the ShowValue is true";
            }
        }

        private void ShowValueMenuItemClick(object sender, EventArgs e)
        {
            if (sender is MenuItem targetMenuItem)
            {
                targetMenuItem.Checked = !targetMenuItem.Checked;
                targetProgressBar.ShowValue = targetMenuItem.Checked;
                descriptionLabel.Text = "ValueAsPerCent describes the value should be reflected inside the component";
            }
        }

        private void BtnClick(object sender, System.EventArgs e)
        {
            var value = targetProgressBar.Value;
            value += Convert.ToInt32((sender as Button).Tag);
            if (value < 0)
            {
                value = 0;
            }

            if (value > targetProgressBar.Maximum)
            {
                value = targetProgressBar.Maximum;
            }

            targetProgressBar.Value = value;
            currentValueLabel.Text = $"Val: {value}";
        }
    }
}
