using System;
using ClForms.Common.EventArgs;
using ClForms.Themes;

namespace ClForms.Abstractions
{
    /// <summary>
    /// Specifies an interaction contract for all elements that take input focus
    /// </summary>
    public interface IFocusableControl
    {
        /// <summary>
        /// Handles a keystroke
        /// </summary>
        void InputAction(ConsoleKeyInfo keyInfo);

        /// <summary>
        /// Gets or sets focus value of component
        /// </summary>
        bool IsFocus { get; set; }

        /// <summary>
        /// Indicates whether component focus can be set
        /// </summary>
        bool CanFocus();

        /// <summary>
        /// Sets input focus to current item
        /// </summary>
        void SetFocus();

        /// <summary>
        /// Focused component background color
        /// </summary>
        Color FocusBackground { get; set; }

        /// <summary>
        /// Focused component text color
        /// </summary>
        Color FocusForeground { get; set; }

        /// <summary>
        /// Component id
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Gets or sets the sequence for moving the TAB key between the controls inside the container
        /// </summary>
        int TabIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can focus on the given control using the TAB key
        /// </summary>
        bool TabStop { get; set; }

        #region Events

        /// <summary>
        /// Input focus event
        /// </summary>
        event EventHandler<System.EventArgs> OnEnter;

        /// <summary>
        /// Input focus loss event
        /// </summary>
        event EventHandler<System.EventArgs> OnLeave;

        /// <summary>
        /// Occurs when a property value of <see cref="TabIndex"/> changes
        /// </summary>
        event EventHandler<PropertyChangedEventArgs<int>> OnTabIndexChanged;

        /// <summary>
        /// Occurs when a property value of <see cref="TabStop"/> changes
        /// </summary>
        event EventHandler<PropertyChangedEventArgs<bool>> OnTabStopChanged;

        #endregion
    }
}
