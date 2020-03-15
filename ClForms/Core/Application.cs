using System;
using System.Collections.Generic;
using ClForms.Abstractions;
using ClForms.Abstractions.Core;
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
        internal static IApp Handler { get; set; }

        /// <summary>
        /// Gets startup parameters
        /// </summary>
        public static IList<string> StartupParameters { get; internal set; }

        /// <summary>
        /// Gets a mechanism for retrieving a service object
        /// </summary>
        public static IServiceProvider ServiceProvider { get; internal set; }

        /// <inheritdoc cref="ISystemColors"/>
        public static ISystemColors SystemColors =
            new Lazy<ISystemColors>(() => ServiceProvider.GetService<ISystemColors>()).Value;

        /// <inheritdoc cref="IEnvironment"/>
        public static IEnvironment Environment =
            new Lazy<IEnvironment>(() => ServiceProvider.GetService<IEnvironment>()).Value;

        /// <summary>
        /// Processes all Windows messages currently in the message queue
        /// </summary>
        public static void DoEvents() => Handler.DoEvents();

        /// <inheritdoc cref="IApp.Terminate"/>
        public static void Terminate() => Handler.Terminate();
    }
}
