using ClForms.Common;
using ClForms.Elements;
using ClForms.Elements.Menu;
using ClForms.Themes;

namespace PanelsApp.Forms
{
    public partial class DockPanelForm: Window
    {
        private readonly MenuItem mainMenuPropertyItem;

        public DockPanelForm(MenuItem mainMenuPropertyItem)
        {
            this.mainMenuPropertyItem = mainMenuPropertyItem;
            InitializeComponent();
            PrepareMenuItems();
            CreateSet1();
        }

        private void PrepareMenuItems()
        {
            mainMenuPropertyItem.Items.Clear();
            var setsMenuItem = new MenuItem("Sets");
            for (var i = 1; i <= 3; i++)
            {
                var item = new MenuItem($"Set{i}")
                {
                    Tag = i
                };
                item.OnClick += SetClick;
                setsMenuItem.Items.Add(item);
            }

            mainMenuPropertyItem.Items.Add(setsMenuItem);
            mainMenuPropertyItem.Items.Add(new SeparatorMenuItem());
            var lastChildFillMenuItem = new MenuItem("LastChildFill");
            lastChildFillMenuItem.OnClick += LastChildFillClick;
            mainMenuPropertyItem.Items.Add(lastChildFillMenuItem);
        }

        private void SetClick(object sender, System.EventArgs e)
        {
            var index = (int) ((MenuItem) sender).Tag;
            if (index == 1)
            {
                CreateSet1();
            }
            else if (index == 2)
            {
                CreateSet2();
            }
            else
            {
                CreateSet3();
            }
            descriptionLabel.Text = "When call AddContent method of DockPanel, specify the position and manner in which a control is docked";
        }

        private void LastChildFillClick(object sender, System.EventArgs e)
        {
            targetPanel.LastChildFill = !targetPanel.LastChildFill;
            ((MenuItem) sender).Checked = targetPanel.LastChildFill;
            descriptionLabel.Text = "LastChildFill Gets or sets a value that indicates whether the last child element within a DockPanel stretches to fill the remaining available space";
        }

        private void CreateSet1()
        {
            targetPanel.RemoveAllContent();
            targetPanel.AddContent(new Label("Content1")
            {
                Background = Color.Magenta,
                Foreground = Color.White,
                Margin = new Thickness(1, 1, 1, 0),
            }, Dock.Top);
            targetPanel.AddContent(new Label("Content2")
            {
                Background = Color.Magenta,
                Foreground = Color.White,
                Margin = new Thickness(1),
            }, Dock.Left);
            targetPanel.AddContent(new Label("Content3")
            {
                Background = Color.Magenta,
                Foreground = Color.White,
                Margin = new Thickness(1),
            }, Dock.Right);
        }

        private void CreateSet2()
        {
            targetPanel.RemoveAllContent();
            targetPanel.AddContent(new Label("Content1")
            {
                Background = Color.Magenta,
                Foreground = Color.White,
                Margin = new Thickness(1),
            }, Dock.Left);
            targetPanel.AddContent(new Label("Content2")
            {
                Background = Color.Magenta,
                Foreground = Color.White,
                Margin = new Thickness(1),
            }, Dock.Bottom);
            targetPanel.AddContent(new Label("Content3")
            {
                Background = Color.Magenta,
                Foreground = Color.White,
                Margin = new Thickness(1, 1, 1, 0),
            }, Dock.Left);
        }

        private void CreateSet3()
        {
            targetPanel.RemoveAllContent();
            targetPanel.AddContent(new Label("Content1")
            {
                Background = Color.Magenta,
                Foreground = Color.White,
                Margin = new Thickness(1),
            }, Dock.Right);
            targetPanel.AddContent(new Label("Content2")
            {
                Background = Color.Magenta,
                Foreground = Color.White,
                Margin = new Thickness(1),
            }, Dock.Right);
            targetPanel.AddContent(new Label("Content3")
            {
                Background = Color.Magenta,
                Foreground = Color.White,
                Margin = new Thickness(1),
            }, Dock.Left);
        }
    }
}
