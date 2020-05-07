using System.IO;
using ClForms.Elements;

namespace MazeCommon
{
    /// <summary>
    /// Open file window
    /// </summary>
    public partial class OpenFileWindow: Window
    {
        public OpenFileWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Files folder
        /// </summary>
        public string TargetFolder { get; set; }

        /// <summary>
        /// Name of the selected file
        /// </summary>
        public string FileName { get; private set; }

        private void OpenFileWindowActivated(object sender, System.EventArgs e)
        {
            listBox1.BeginUpdate();
            foreach (var file in Directory.GetFiles(TargetFolder, $"*{Consts.MazeMapFileExtension}", SearchOption.TopDirectoryOnly))
            {
                listBox1.Items.Add(Path.GetFileNameWithoutExtension(file));
            }
            listBox1.EndUpdate();
        }

        private void ListBoxSelectedIndexChanged(object sender, ClForms.Common.EventArgs.PropertyChangedEventArgs<int> e)
        {
            FileName = Path.Combine(TargetFolder, $"{listBox1.Items[listBox1.SelectedIndex]}{Consts.MazeMapFileExtension}");
            button1.IsDisabled = false;
        }
    }
}
