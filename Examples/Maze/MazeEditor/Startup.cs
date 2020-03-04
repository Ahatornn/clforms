using ClForms.Abstractions.Core;
using Microsoft.Extensions.DependencyInjection;

namespace MazeEditor
{
    /// <inheritdoc cref="IStartup"/>
    public class Startup: IStartup
    {
        /// <inheritdoc cref="IStartup.ConfigureServices"/>
        public void ConfigureServices(IServiceCollection services)
        {
            // Optional
            // Called by the application to configure the app's services.
        }
    }
}
