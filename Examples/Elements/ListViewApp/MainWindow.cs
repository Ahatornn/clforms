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


            listView1.Text = " C:\\ ";
            listView1.SummaryText = "This is a summary of the left panel and it's testing of very longest text ever";
            listView1.Columns = 2;
            listView1.ColumnHeaders.Add("AAA");
            listView1.ColumnHeaders.Add("BBB");

            listView2.Text = " C:\\TEMP ";
            listView2.SummaryText = "This is a summary of the right panel";
            listView2.Columns = 3;
            listView2.ColumnHeaders.Add("Name1");
            listView2.ColumnHeaders.Add("Name2");
        }
    }
}
