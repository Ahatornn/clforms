using System;
using ClForms.Common;
using MazeCommon;

namespace MazeEditor.Models
{
    public class MazeMapItemEventArgs: EventArgs
    {
        public Point Point { get; }
        public MapItem Item { get; }

        internal MazeMapItemEventArgs(Point point, MapItem item)
        {
            Point = point;
            Item = item;
        }
    }
}
