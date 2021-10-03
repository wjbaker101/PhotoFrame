using Microsoft.Extensions.DependencyInjection;
using PhotoFrame.Windows;
using System.Windows;
using PhotoFrame.Windows.Main;

namespace PhotoFrame
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();

            mainWindow?.Show();
        }
    }
}