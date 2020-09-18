using System;
using System.Collections.Generic;
using System.Text;

namespace ListViewApp
{
    public class DiskItem
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsFolder { get; set; }

        public override string ToString() => Name;
    }
}
