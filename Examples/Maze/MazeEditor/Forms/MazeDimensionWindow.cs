using System;
using ClForms.Common;
using ClForms.Common.EventArgs;
using ClForms.Elements;

namespace MazeEditor.Forms
{
    public partial class MazeDimensionWindow: Window
    {
        public MazeDimensionWindow()
        {
            InitializeComponent();
        }

        private void KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (e.KeyInfo.Key == ConsoleKey.DownArrow)
            {
                if (radioButton1.IsFocus)
                {
                    radioButton2.SetFocus();
                }
                else if (radioButton2.IsFocus)
                {
                    radioButton3.SetFocus();
                }
                else
                {
                    radioButton1.SetFocus();
                }
            }

            if (e.KeyInfo.Key == ConsoleKey.UpArrow)
            {
                if (radioButton1.IsFocus)
                {
                    radioButton3.SetFocus();
                }
                else if (radioButton2.IsFocus)
                {
                    radioButton1.SetFocus();
                }
                else
                {
                    radioButton2.SetFocus();
                }
            }
        }

        public Size SelectedDimension
        {
            get
            {
                if (radioButton2.Checked)
                {
                    return (Size) radioButton2.Tag;
                }

                if (radioButton3.Checked)
                {
                    return (Size) radioButton3.Tag;
                }

                return (Size) radioButton1.Tag;
            }
        }
    }
}
