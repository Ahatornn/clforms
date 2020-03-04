using System;
using System.Text;
using ClForms.Abstractions.Core;
using ClForms.Themes;

namespace ClForms.Core
{
    /// <summary>
    /// Resolver of <see cref="IPseudographicsProvider"/> for console wondow
    /// </summary>
    internal class PseudographicsProvider: IPseudographicsProvider
    {
        /// <inheritdoc cref="IPseudographicsProvider.SetCursorPosition(int, int)"/>
        public void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);

        /// <inheritdoc cref="IPseudographicsProvider.WriteLine(object)"/>
        public void WriteLine(object value) => Console.WriteLine(value);

        /// <inheritdoc cref="IPseudographicsProvider.Write(char)"/>
        public void Write(char value) => Console.Write(value);

        /// <inheritdoc cref="IPseudographicsProvider.Write(string)"/>
        public void Write(string value) => Console.Write(value);

        /// <inheritdoc cref="IPseudographicsProvider.WindowHeight"/>
        public int WindowHeight
        {
            get => Console.WindowHeight;
            set => Console.WindowHeight = value;
        }

        /// <inheritdoc cref="IPseudographicsProvider.WindowWidth"/>
        public int WindowWidth
        {
            get => Console.WindowWidth;
            set => Console.WindowWidth = value;
        }

        /// <inheritdoc cref="IPseudographicsProvider.KeyAvailable"/>
        public bool KeyAvailable => Console.KeyAvailable;

        /// <inheritdoc cref="IPseudographicsProvider.ReadKey"/>
        public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);

        /// <inheritdoc cref="IPseudographicsProvider.Title"/>
        public string Title
        {
            get => Console.Title;
            set => Console.Title = value;
        }

        /// <inheritdoc cref="IPseudographicsProvider.OutputEncoding"/>
        public Encoding OutputEncoding
        {
            get => Console.OutputEncoding;
            set => Console.OutputEncoding = value;
        }

        /// <inheritdoc cref="IPseudographicsProvider.CursorVisible"/>
        public bool CursorVisible
        {
            get => Console.CursorVisible;
            set => Console.CursorVisible = value;
        }

        /// <inheritdoc cref="IPseudographicsProvider.BackgroundColor"/>
        public Color BackgroundColor
        {
            get => (Color) (int) Console.BackgroundColor;
            set
            {
                if (value != Color.NotSet)
                {
                    Console.BackgroundColor = (ConsoleColor) value;
                }
            }
        }

        /// <inheritdoc cref="IPseudographicsProvider.ForegroundColor"/>
        public Color ForegroundColor
        {
            get => (Color) (int) Console.ForegroundColor;
            set
            {
                if (value != Color.NotSet)
                {
                    Console.ForegroundColor = (ConsoleColor) value;
                }
            }
        }

        /// <inheritdoc cref="IPseudographicsProvider.Clear"/>
        public void Clear() => Console.Clear();
    }
}
