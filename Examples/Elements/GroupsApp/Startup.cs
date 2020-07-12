﻿using ClForms.Abstractions.Core;
using ClForms.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;

namespace GroupsApp
{
    /// <inheritdoc cref="IStartup"/>
    public class Startup : IStartup
    {
        /// <inheritdoc cref="IStartup.ConfigureServices"/>
        public void ConfigureServices(IServiceCollection services)
        {
            // Optional
            // Called by the application to configure the app's services
        }

        /// <inheritdoc cref="IStartup.Configure"/>
        public void Configure(IServiceProvider provider)
        {
            // Optional
            // Called by the application to configure specified services

            provider.SetConfigure<IPseudographicsProvider>(configure =>
            {
                configure.OutputEncoding = Encoding.GetEncoding("Utf-8");
            });
        }
    }
}
