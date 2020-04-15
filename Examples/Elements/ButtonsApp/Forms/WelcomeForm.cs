using System.Collections.Generic;
using ClForms.Elements;

namespace ButtonsApp.Forms
{
    public partial class WelcomeForm: Window
    {
        private readonly IEnumerable<string> controlList;
        public WelcomeForm()
        {
            controlList = new List<string>
            {
                "Button",
                "CheckBox",
                "RadioButton",
                "MessageBox",
            };
            InitializeComponent();
        }
    }
}
