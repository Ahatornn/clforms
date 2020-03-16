using ClForms.Abstractions.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ClForms.Core
{
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// Specify the startup type to be used for application
        /// </summary>
        public static IAppBuilder UseStartup<T>(this IAppBuilder builder) where T : IStartup
            => builder?.ConfigureServices(service =>
            {
                var startupConfig = (T) Activator.CreateInstance(typeof(T));
                startupConfig.ConfigureServices(service);
                service.AddSingleton(typeof(IStartup), sp => startupConfig);
            });
    }
}