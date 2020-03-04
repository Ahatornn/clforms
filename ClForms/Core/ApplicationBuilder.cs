using System;
using System.Collections.Generic;
using ClForms.Abstractions;
using ClForms.Abstractions.Core;
using ClForms.Themes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ClForms.Core
{
    /// <inheritdoc cref="IAppBuilder"/>
    internal class ApplicationBuilder: IAppBuilder
    {
        private readonly string[] startArgs;
        private readonly List<Action<IServiceCollection>> configureServicesDelegates;

        /// <summary>
        /// Initialize a new instance <see cref="ApplicationBuilder"/>
        /// </summary>
        internal ApplicationBuilder(string[] startArgs)
        {
            this.startArgs = startArgs;
            configureServicesDelegates = new List<Action<IServiceCollection>>();
        }

        /// <inheritdoc cref="IAppBuilder.ConfigureServices"/>
        public IAppBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            if (configureServices == null)
            {
                throw new ArgumentNullException(nameof(configureServices));
            }
            configureServicesDelegates.Add(configureServices);
            return (IAppBuilder) this;
        }

        /// <inheritdoc cref="IAppBuilder.SetMainWindow{TWindow}"/>
        public IAppBuilder SetMainWindow<TWindow>() where TWindow : class => throw new NotImplementedException();

        /// <inheritdoc cref="IAppBuilder.Build"/>
        public IApp Build()
        {
            var collections = new ServiceCollection();
            foreach (var action in configureServicesDelegates)
            {
                action(collections);
            }

            collections.TryAddScoped<ISystemColors, DefaultSystemColors>();
            collections.TryAddScoped<IEventLoop, EventLoop>();
            collections.TryAddScoped<IPseudographicsProvider, PseudographicsProvider>();

            Application.StartupParameters = startArgs;
            var provider = collections.BuildServiceProvider();
            Application.ServiceProvider = provider;

            var handler = new ApplicationHandler(provider.GetService<IEventLoop>());
            Application.Handler = handler;

            return handler;
        }
    }
}
