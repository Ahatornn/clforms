using System;
using System.Linq;

namespace ClForms.Helpers
{
    /// <summary>
    /// Helper display class for <see cref="ConsoleKey" />
    /// </summary>
    internal static class ConsoleKeyVisualHelper
    {
        /// <summary>
        /// Forms a display string of <see cref="ConsoleKey"/>
        /// </summary>
        internal static string ToPopupMenuString(this ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.D0:
                case ConsoleKey.D1:
                case ConsoleKey.D2:
                case ConsoleKey.D3:
                case ConsoleKey.D4:
                case ConsoleKey.D5:
                case ConsoleKey.D6:
                case ConsoleKey.D7:
                case ConsoleKey.D8:
                case ConsoleKey.D9:
                    return key.ToString().Last().ToString();
                default:
                    return key.ToString();
            }
        }
    }
}
