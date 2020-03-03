using System;
using System.Collections.Generic;
using ClForms.Abstractions.Core;
using Microsoft.Extensions.DependencyInjection;

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

        /// <inheritdoc cref="IAppBuilder.Build"/>
        public IApp Build() => throw new NotImplementedException();
    }
}
