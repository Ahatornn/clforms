using System;
using ClForms.Common;
using ClForms.Elements;
using ClForms.Elements.Menu;
using ClForms.Themes;
using ListViewApp.Models;

namespace ListViewApp.Forms
{
    /// <summary>
    /// The main Window of App
    /// </summary>
    public partial class HeaderForm: Window
    {
        private readonly MenuItem mainMenuPropertyItem;
        private bool drawingFormat;

        /// <summary>
        /// Initialize a new instance <see cref="HeaderForm"/>
        /// </summary>
        public HeaderForm(MenuItem mainMenuPropertyItem)
        {
            this.mainMenuPropertyItem = mainMenuPropertyItem;
            InitializeComponent();
            PrepareMenuItems();
            drawingFormat = false;
        }

        private void PrepareMenuItems()
        {
            mainMenuPropertyItem.Items.Clear();
            var bcMenuItem = new MenuItem("Header foreground");
            foreach (var item in Enum.GetValues(typeof(Color)))
            {
                if ((Color) item == Color.NotSet)
                {
                    continue;
                }
                var menuItem = new MenuItem(item.ToString()) { Tag = item, };
                menuItem.OnClick += HeaderForegroundClick;
                bcMenuItem.Items.Add(menuItem);
            }
            mainMenuPropertyItem.Items.Add(bcMenuItem);
            mainMenuPropertyItem.Items.Add(new SeparatorMenuItem());

            var setsMenuItem = new MenuItem("Header sets");

            var setOneMenuItem = new MenuItem("Set one (3 col)");
            setOneMenuItem.OnClick += SetOneMenuItemClick;
            setsMenuItem.Items.Add(setOneMenuItem);

            var setTwoMenuItem = new MenuItem("Set two (5 col)");
            setTwoMenuItem.OnClick += SetTwoMenuItemClick;
            setsMenuItem.Items.Add(setTwoMenuItem);

            mainMenuPropertyItem.Items.Add(setsMenuItem);
            mainMenuPropertyItem.Items.Add(new SeparatorMenuItem());

            var itemDrawMenuItem = new MenuItem("Custom item format");
            itemDrawMenuItem.OnClick += ItemDrawMenuItemClick;
            mainMenuPropertyItem.Items.Add(itemDrawMenuItem);
        }

        private void ItemDrawMenuItemClick(object sender, EventArgs e)
        {
            drawingFormat = !drawingFormat;
            (sender as MenuItem).Checked = drawingFormat;
            descriptionLabel.Text = "You can change background and foreground item when it draw";
            codeLabel.Lines = new[]
            {
                "listView.OnItemDraw += ItemDraw;",
                "…",
                "…ItemDraw(sender, e)",
                "{",
                "    if (condition)",
                "    {",
                "        e.Foreground = Color.White;",
                "    }",
                "}",
            };
            targetListView.InvalidateMeasure();
        }

        private void SetTwoMenuItemClick(object sender, EventArgs e)
        {
            if (targetListView.ColumnHeaders.Count != 5)
            {
                targetListView.ColumnHeaders.Clear();
                targetListView.ColumnHeaders.Add("Country", x => x.Country);
                targetListView.ColumnHeaders.Add("Alpha", x => x.Alpha3, 3);
                targetListView.ColumnHeaders.Add("Gdp", x => x.Gdp.ToString(), 6, TextAlignment.Right);
                targetListView.ColumnHeaders.Add("Population", x => x.Population.ToString(), 9, TextAlignment.Right);
                targetListView.ColumnHeaders.Add("%", x => x.Percentage.ToString(), 6, TextAlignment.Center);
                descriptionLabel.Text = "You cannot change any properties of Header after creations. If it needed: remove old Header and create a newer";
                codeLabel.Lines = new[]
                {
                    "Add(string text)",
                    "Add(string text, int? width)",
                    "Add(string text, Func<T, string> displayMember)",
                    "Add(string text, Func<T, string> displayMember, int? width, TextAlignment alignment)",
                };
            }
        }

        private void SetOneMenuItemClick(object sender, EventArgs e)
        {
            if (targetListView.ColumnHeaders.Count != 3)
            {
                targetListView.ColumnHeaders.Clear();
                targetListView.ColumnHeaders.Add("Country", x => x.Country);
                targetListView.ColumnHeaders.Add("Gdp", x => x.Gdp.ToString(), 6, TextAlignment.Right);
                targetListView.ColumnHeaders.Add("Ppl", x => x.Population.ToString(), 9, TextAlignment.Right);
                descriptionLabel.Text = "You cannot change any properties of Header after creations. If it needed: remove old Header and create a newer";
                codeLabel.Lines = new[]
                {
                    "Add(string text)",
                    "Add(string text, int? width)",
                    "Add(string text, Func<T, string> displayMember)",
                    "Add(string text, Func<T, string> displayMember, int? width, TextAlignment alignment)",
                };
            }
        }

        private void HeaderForegroundClick(object sender, EventArgs e)
        {
            targetListView.HeaderForeground = (Color) (sender as MenuItem).Tag;
            descriptionLabel.Text = "HeaderForeground gets or sets a color that describes the foreground of a control headers";
            codeLabel.Lines = new []
            {
                "…",
                $"listView1.HeaderForeground = Color.{targetListView.HeaderForeground.ToString()};",
                "…",
            };
        }

        private void TargetListViewItemDraw(object sender, ClForms.Common.EventArgs.ListBoxItemStyleEventArgs<CountryInfo> e)
        {
            if (!drawingFormat)
            {
                return;
            }

            if (targetListView.Items.IndexOf(e.Item) % 2 == 0)
            {
                e.Foreground = Color.White;
            }
        }
    }
}
