using System;
using System.Text;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Helpers;
using ClForms.Themes;

namespace ClForms.Core.Contexts
{
    internal class ColorDevice: ArrayDevice<Color>
    {
        public ColorDevice(Size size) : base(size) { }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var row = 0; row < device.GetLength(ArrayDimension.Row); row++)
            {
                for (var col = 0; col < device.GetLength(ArrayDimension.Column); col++)
                {
                    switch (device[row, col])
                    {
                        case Color.Black:
                        case Color.Blue:
                        case Color.DarkGreen:
                        case Color.DarkBlue:
                        case Color.DarkMagenta:
                        case Color.DarkRed:
                            sb.Append('█');
                            break;

                        case Color.Magenta:
                        case Color.Green:
                        case Color.Red:
                        case Color.DarkGray:
                        case Color.DarkYellow:
                        case Color.DarkCyan:
                            sb.Append('▓');
                            break;

                        case Color.Cyan:
                        case Color.Gray:
                            sb.Append('▒');
                            break;

                        case Color.Yellow:
                        case Color.White:
                            sb.Append('░');
                            break;

                        default:
                            sb.Append(' ');
                            break;
                    }
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
