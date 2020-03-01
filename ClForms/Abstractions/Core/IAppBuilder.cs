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
    }
}
