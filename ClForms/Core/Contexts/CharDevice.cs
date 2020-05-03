using System;
using System.Text;
using ClForms.Abstractions.Engine;
using ClForms.Common;
using ClForms.Helpers;

namespace ClForms.Core.Contexts
{
    internal class CharDevice: ArrayDevice<char>
    {
        public CharDevice(Size size) : base(size) { }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var row = 0; row < device.GetLength(ArrayDimension.Row); row++)
            {
                for (var col = 0; col < device.GetLength(ArrayDimension.Column); col++)
                {
                    if(device[row, col] == '\0')
                    {
                        sb.Append(' ');
                    }
                    else
                    {
                        sb.Append(device[row, col]);
                    }
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
