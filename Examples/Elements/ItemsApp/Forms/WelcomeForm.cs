using System.Collections.Generic;
using ClForms.Elements;

namespace ItemsApp.Forms
{
    public partial class WelcomeForm: Window
    {
        private readonly IEnumerable<string> controlList;

        public WelcomeForm()
        {
            controlList = new List<string>
            {
                "ListBox",
            };
            InitializeComponent();
        }
    }
}
