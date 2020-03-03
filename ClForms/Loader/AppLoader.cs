using System;
using ClForms.Abstractions.Core;
using ClForms.Core;

namespace ClForms.Loader
{
    /// <summary>
    /// Loader of the application
    /// </summary>
    public static class AppLoader
    {
        /// <summary>
        /// Create <see cref="IAppBuilder"/> with default value
        /// </summary>
        public static IAppBuilder CreateDefaultBuilder()
            => CreateDefaultBuilder(Array.Empty<string>());

        /// <summary>
        /// Create <see cref="IAppBuilder"/> with startup arguments
        /// </summary>
        public static IAppBuilder CreateDefaultBuilder(string[] args)
            => new ApplicationBuilder(args);
    }
}
