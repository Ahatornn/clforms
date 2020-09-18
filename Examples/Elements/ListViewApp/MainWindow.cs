using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClForms.Common;
using ClForms.Elements;
using ClForms.Themes;

namespace ListViewApp
{
    /// <summary>
    /// The main Window of App
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initialize a new instance <see cref="MainWindow"/>
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            var path = "C:\\Windows";
            var items = GetDiskItems(path).ToList();

            listView1.Text = $" {path} ";
            listView1.SummaryText = "This is a summary of the left panel and it's testing of very longest text ever";
            listView1.ColumnHeaders.Add("Name");
            listView1.ColumnHeaders.Add("Size", x => GetSizeText(x), 9, TextAlignment.Right);
            listView1.ColumnHeaders.Add("Date", x => x.DateTime.ToString("dd.MM.yy"), 10, TextAlignment.Center);
            listView1.ColumnHeaders.Add("Time", x => x.DateTime.ToString("HH:mm"), 6, TextAlignment.Right);
            listView1.Items.AddRange(items);
            listView1.OnSelectedIndexChanged += SelectedIndexChanged;
            listView1.OnItemDraw += ListView2StyleItem;

            listView2.Text = $" {path} ";
            listView2.SummaryText = "This is a summary of the right panel";
            listView2.Columns = 4;
            listView2.Items.AddRange(items);
            listView2.OnItemDraw += ListView2StyleItem;
            listView2.OnSelectedIndexChanged += SelectedIndexChanged;
            listView2.OnItemDrawing += ListView2ItemDrawing;
        }

        private void ListView2ItemDrawing(object sender, ClForms.Common.EventArgs.ListBoxItemDrawingEventArgs<DiskItem> e)
        {
            if (!e.Item.IsFolder)
            {
                if(e.Item.Name.All(x => x != '.'))
                {
                    return;
                }
                var result = e.Item.Name.Substring(0, e.Item.Name.LastIndexOf('.'));
                if(result.Length + 4 > e.DrawingArea.Width)
                {
                    result = result.Substring(0, e.DrawingArea.Width - 4) + "…";
                }
                var extension = e.Item.Name.Substring(e.Item.Name.IndexOf('.') + 1);
                if(extension.Length > 3)
                {
                    extension = extension.Substring(0, 2) + "…";
                }
                if(result.Length + extension.Length < e.DrawingArea.Width)
                {
                    result = result + new string(' ', e.DrawingArea.Width - (result.Length + extension.Length)) + extension;
                }
                else
                {
                    result = result + extension;
                }
                if(extension == "exe")
                {
                    e.Context.DrawText(result.Substring(0, result.Length - 3), e.Background, e.Foreground);
                    e.Context.DrawText(extension, e.Background, Color.Red);
                }
                else
                {
                    e.Context.DrawText(result, e.Background, extension == "log" ? Color.Black : e.Foreground);
                }
                e.Handled = true;
            }
        }

        private void SelectedIndexChanged(object sender, ClForms.Common.EventArgs.PropertyChangedEventArgs<int> e)
        {
            var targetList = sender as ListView<DiskItem>;
            if(e.NewValue == -1)
            {
                targetList.SummaryText = string.Empty;
            }
            else
            {
                targetList.SummaryText = targetList.Items[e.NewValue].Name;
            }
        }

        private void ListView2StyleItem(object sender, ClForms.Common.EventArgs.ListBoxItemStyleEventArgs<DiskItem> e)
        {
            if (e.Item.IsFolder)
            {
                e.Foreground = Color.Yellow;
            }
            else
            {
                e.Foreground = Color.Gray;
            }
        }

        private IEnumerable<DiskItem> GetDiskItems(string path)
        {
            var parent = Directory.GetParent(path);
            if (parent != null)
            {
                yield return new DiskItem
                {
                    Path = parent.FullName,
                    Name = "..",
                    IsFolder = true,
                    DateTime = Directory.GetCreationTime(parent.FullName),
                };
            }
            foreach (var directoryName in Directory.EnumerateDirectories(path))
            {
                yield return new DiskItem
                {
                    Path = directoryName,
                    Name = directoryName.Substring(directoryName.LastIndexOf('\\') + 1),
                    IsFolder = true,
                    DateTime = Directory.GetCreationTime(directoryName),
                };
            }
            foreach (var file in Directory.GetFiles(path))
            {
                yield return new DiskItem
                {
                    Path = Path.GetFullPath(file),
                    Name = Path.GetFileName(file),
                    IsFolder = false,
                    DateTime = File.GetCreationTime(file),
                    Size = new FileInfo(file).Length,
                };
            }
        }

        private static string GetSizeText(DiskItem item)
        {
            if(item == null)
            {
                return string.Empty;
            }
            if (item.IsFolder)
            {
                return "►SUB-DIR◄";
            }
            if(item.Size < 1024)
            {
                return $"{item.Size} b ";
            }
            if(item.Size < 1024 * 1024)
            {
                return $"{item.Size / 1024} Kb";
            }

            if (item.Size < 1024 * 1024 * 1024)
            {
                return $"{item.Size / (1024 * 1024)} Mb";
            }

            return $"{item.Size / (1024 * 1024 * 1024)} Gb";
        }
    }
}
