using System;
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
            var summaryText = "Use [←], [↑], [→], [↓], [Home], [End] keys for navigation";

            listView1.Text = $" {path} ";
            listView1.ShowSummary = true;
            listView1.SummaryText = summaryText;
            listView1.ColumnHeaders.Add("Name");
            listView1.ColumnHeaders.Add("Size", x => GetSizeText(x), 9, TextAlignment.Right);
            listView1.ColumnHeaders.Add("Date", x => x.DateTime.ToString("dd.MM.yy"), 10, TextAlignment.Center);
            listView1.ColumnHeaders.Add("Time", x => x.DateTime.ToString("HH:mm"), 6, TextAlignment.Right);
            listView1.Items.AddRange(items);
            listView1.SelectedIndex = 0;
            listView1.OnSelectedIndexChanged += SelectedIndexChanged;
            listView1.OnItemDraw += ListView2StyleItem;
            listView1.OnItemClick += ListViewItemClick;

            listView2.Text = $" {path} ";
            listView2.ShowSummary = true;
            listView2.SummaryText = summaryText;
            listView2.Columns = 4;
            listView2.Items.AddRange(items);
            listView2.SelectedIndex = 0;
            listView2.OnItemDraw += ListView2StyleItem;
            listView2.OnSelectedIndexChanged += SelectedIndexChanged;
            listView2.OnItemDrawing += ListView2ItemDrawing;
            listView2.OnItemClick += ListViewItemClick;
        }

        private void ListViewItemClick(object sender, DiskItem e)
        {
            if(e == null || !e.IsFolder)
            {
                return;
            }
            var targetList = sender as ListView<DiskItem>;
            targetList.Items.Clear();
            var items = GetDiskItems(e.Path).ToList();
            targetList.Items.AddRange(items);
            if (items.Any())
            {
                targetList.SelectedIndex = 0;
            }
            targetList.Text = $" {e.Path} ";
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
            targetList.SummaryText = e.NewValue == -1
                ? string.Empty
                : targetList.Items[e.NewValue].Name;
        }

        private void ListView2StyleItem(object sender, ClForms.Common.EventArgs.ListBoxItemStyleEventArgs<DiskItem> e)
            => e.Foreground = e.Item.IsFolder ? Color.Yellow : Color.Gray;

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

            var directoryNames = Array.Empty<string>();
            var files = Array.Empty<string>();
            try
            {
                directoryNames = Directory.EnumerateDirectories(path).ToArray();
                files = Directory.GetFiles(path);
            }
            catch (Exception e)
            {
                MessageBox.ShowError(e.Message);
            }

            foreach (var directoryName in directoryNames)
            {
                yield return new DiskItem
                {
                    Path = directoryName,
                    Name = directoryName.Substring(directoryName.LastIndexOf('\\') + 1),
                    IsFolder = true,
                    DateTime = Directory.GetCreationTime(directoryName),
                };
            }
            foreach (var file in files)
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
