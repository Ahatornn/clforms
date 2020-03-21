using ClForms.Core;
using ClForms.Loader;

namespace WindowApp
{
    class Program
    {
        private static void Main(string[] args)
            => AppLoader.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .Start(new MainWindow());
    }
}
