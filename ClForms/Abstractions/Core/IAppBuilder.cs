using System;
using Microsoft.Extensions.DependencyInjection;

namespace ClForms.Abstractions.Core
{
    /// <summary>
    /// A builder for application
    /// </summary>
    public interface IAppBuilder
    {
        /// <summary>
        /// Builds an <see cref="IApp"/> which specified configuration
        /// </summary>
        IApp Build();

        /// <summary>
        /// Adds a delegate for configuring additional services for the application
        /// </summary>
        IAppBuilder ConfigureServices(Action<IServiceCollection> configureServices);

        /// <summary>
        /// Sets the main window for application
        /// </summary>
        IAppBuilder SetMainWindow<TWindow>() where TWindow : class;
    }
}
