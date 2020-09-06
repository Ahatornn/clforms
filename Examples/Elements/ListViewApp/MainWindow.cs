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
            listView1.ColumnHeaders.Add("Size", x => x.IsFolder ? "►SUB-DIR◄" : x.Size.ToString(), 9);
            listView1.ColumnHeaders.Add("Date", x => x.DateTime.ToString("dd.MM.yy"), 10, TextAlignment.Center);
            listView1.ColumnHeaders.Add("Time", x => x.DateTime.ToString("HH:mm"), 6, TextAlignment.Right);
            listView1.Items.AddRange(items);

            listView2.Text = $" {path} ";
            listView2.SummaryText = "This is a summary of the right panel";
            listView2.Columns = 4;
            listView2.Items.AddRange(items);
        }

        private IEnumerable<DiskItem> GetDiskItems(string path)
        {
            foreach (var directoryName in Directory.EnumerateDirectories(path))
            {
                yield return new DiskItem()
                {
                    Path = directoryName,
                    Name = directoryName.Substring(directoryName.LastIndexOf('\\') + 1),
                    IsFolder = true,
                    DateTime = Directory.GetCreationTime(directoryName),
                };
            }
        }
    }
}
