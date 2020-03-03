using Microsoft.Extensions.DependencyInjection;

namespace ClForms.Abstractions.Core
{
    /// <summary>
    /// Represents platform specific configuration that will be applied to a <see cref="IAppBuilder" /> when building an <see cref="IApp" />.
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// This API may change or be removed in future releases
        /// </summary>
        void ConfigureServices(IServiceCollection services);
    }
}
