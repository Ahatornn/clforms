using System;
using ClForms.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ClForms.Core
{
    /// <summary>
    /// Provides <see langword="static"/> methods and properties to manage an application, such as methods to start
    /// and stop an application, to process Windows messages, and properties to get information about an application.
    /// This class cannot be inherited.
    /// </summary>
    public static class Application
    {
        internal static ApplicationHandler Handler;

        /// <summary>
        /// Gets startup parameters
        /// </summary>
        public static string[] StartupParameters { get; internal set; }

        /// <summary>
        /// Gets a mechanism for retrieving a service object
        /// </summary>
        public static IServiceProvider ServiceProvider { get; internal set; }

        /// <inheritdoc cref="ISystemColors"/>
        public static ISystemColors SystemColors =
            new Lazy<ISystemColors>(() => ServiceProvider.GetService<ISystemColors>()).Value;
    }
}