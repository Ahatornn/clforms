using ClForms.Core;
using ClForms.Loader;

namespace GroupsApp
{
    /// <summary>
    /// Class of entry point
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point of App
        /// </summary>
        private static void Main(string[] args)
            => AppLoader.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .Start(new MainWindow());
    }
}
