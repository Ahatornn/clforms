using System;
using System.Collections.Generic;
using ClForms.Abstractions;
using ClForms.Abstractions.Core;
using ClForms.Loader;
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
            return this;
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

            collections.TryAddSingleton<ISystemColors, DefaultSystemColors>();
            collections.TryAddSingleton<IEventLoop, EventLoop>();
            collections.TryAddSingleton<IPseudographicsProvider, PseudographicsProvider>();
            collections.TryAddSingleton<IControlLifeCycle, ControlLifeCycle>();
            collections.TryAddSingleton<IEnvironment, DefaultEnvironment>();
            collections.TryAddSingleton<IApp, ApplicationHandler>();

            Application.StartupParameters = startArgs;
            var provider = collections.BuildServiceProvider();
            Application.ServiceProvider = provider;

            var handler = provider.GetService<IApp>();
            Application.Handler = handler;
            var sturtup = provider.GetService<IStartup>();
            sturtup?.Configure(provider);
            return handler;
        }
    }
}
