using System;
using ClForms.Abstractions.Core;

namespace ClForms.Core
{
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// Specify the startup type to be used for application
        /// </summary>
        public static IAppBuilder UseStartup<T>(this IAppBuilder builder) where T : IStartup
            => builder.ConfigureServices(service =>
            {
                var startupConfig = (T) Activator.CreateInstance(typeof(T));
                startupConfig.ConfigureServices(service);
            });
    }
}