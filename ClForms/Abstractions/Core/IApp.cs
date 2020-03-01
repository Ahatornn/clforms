using System;

namespace ClForms.Abstractions.Core
{
    /// <summary>
    /// Represents a configured application
    /// </summary>
    public interface IApp: IDisposable
    {
        /// <summary>
        /// Starts application
        /// </summary>
        void Start();
    }
}
