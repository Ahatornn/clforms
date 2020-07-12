using ClForms.Elements;

namespace ClForms.Common.EventArgs
{
    /// <summary>
    /// Selected item of <see cref="CheckBoxGroup"/> changing event options
    /// </summary>
    public class SelectedItemCheckedEventArgs: System.EventArgs
    {
        /// <summary>
        /// Gets the zero-based index of the currently selected item in a <see cref="CheckBoxGroup"/>
        /// </summary>
        public int SelectedIndex { get; }

        /// <summary>
        /// State of <see cref="SelectedIndex"/>
        /// </summary>
        public CheckState State { get; }

        /// <summary>
        /// Initialize a new instance <see cref="SelectedItemCheckedEventArgs"/>
        /// </summary>
        public SelectedItemCheckedEventArgs(int selectedIndex, CheckState state)
        {
            SelectedIndex = selectedIndex;
            State = state;
        }
    }
}
