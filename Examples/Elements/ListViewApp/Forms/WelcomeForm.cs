using System.Collections.Generic;
using ClForms.Elements;

namespace ListViewApp.Forms
{
    /// <summary>
    /// The main Window of App
    /// </summary>
    public partial class WelcomeForm: Window
    {
        private readonly IEnumerable<string> controlList;

        /// <summary>
        /// Initialize a new instance <see cref="WelcomeForm"/>
        /// </summary>
        public WelcomeForm()
        {
            controlList = new List<string>
            {
                "LIstView",
            };
            InitializeComponent();
        }
    }
}
