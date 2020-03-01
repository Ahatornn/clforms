using System;

namespace ClForms.Abstractions
{
    /// <summary>
    /// Defines actions with an element style
    /// </summary>
    public interface IElementStyle<out T>
    {
        /// <summary>
        /// Sets element style
        /// </summary>
        void SetStyle(Action<T> styleAction);
    }
}
