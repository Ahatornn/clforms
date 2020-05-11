using System.IO;
using ClForms.Common.EventArgs;
using ClForms.Elements;
using MazeCommon;

namespace MazeEditor.Forms
{
    public partial class SaveFileWindow: Window
    {
        public SaveFileWindow()
        {
            InitializeComponent();
            FileName = string.Empty;
        }

        public string TargetFolder { get; set; }

        public string FileName { get; private set; }

        private void SaveFileWindowActivated(object sender, System.EventArgs e)
        {
            listBox1.BeginUpdate();
            foreach (var file in Directory.GetFiles(TargetFolder, $"*{Consts.MazeMapFileExtension}", SearchOption.TopDirectoryOnly))
            {
                listBox1.Items.Add(Path.GetFileNameWithoutExtension(file));
            }
            listBox1.EndUpdate();
        }

        private void TextBox1Changed(object sender, PropertyChangedEventArgs<string> e)
        {
            button1.IsDisabled = string.IsNullOrWhiteSpace(e.NewValue) || listBox1.Items.Contains(e.NewValue);
            FileName = e.NewValue;
        }

        private void ListBoxSelectedItemClick(object sender, object e)
            => textBox1.Text = e.ToString();
    }
}
