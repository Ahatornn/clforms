using System;

namespace ClForms.Common.EventArgs
{
    /// <summary>
    /// Key Press Event Options
    /// </summary>
    public sealed class KeyPressedEventArgs: System.EventArgs
    {
        /// <summary>
        /// Key pressed
        /// </summary>
        public ConsoleKeyInfo KeyInfo { get; }

        /// <summary>
        /// Initialize a new instance <see cref="KeyPressedEventArgs"/>
        /// </summary>
        public KeyPressedEventArgs(ConsoleKeyInfo keyInfo)
        {
            KeyInfo = keyInfo;
        }
    }
}
