using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Charites.Windows.Mvc;
using Microsoft.Extensions.Hosting;

namespace WpfMvcApp
{
    internal class WpfMvcAppApplication : Application
    {
        private readonly IHostApplicationLifetime lifetime;

        public WpfMvcAppApplication(IHostApplicationLifetime lifetime, IServiceProvider services)
        {
            this.lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));

            Startup += WpfMvcAppApplication_Startup;
            Exit += WpfMvcAppApplication_Exit;
            DispatcherUnhandledException += WpfMvcAppApplication_DispatcherUnhandledException;

            WpfController.UnhandledException += WpfController_UnhandledException;
            WpfController.ControllerFactory = new WpfMvcAppControllerFactory(services);

            AddResourceDictionary("Resources.xaml");
        }

        private void AddResourceDictionary(string resourceFileName)
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri($"/{Assembly.GetExecutingAssembly().FullName};component/Resources/{resourceFileName}", UriKind.Relative)
            });
        }

        private void WpfMvcAppApplication_Startup(object sender, StartupEventArgs e)
        {
            MainWindow = new Window
            {
                Style = FindResource("MainWindowStyle") as Style,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                DataContext = new MainContent()
            };
            MainWindow.Show();
        }

        private void WpfMvcAppApplication_Exit(object sender, ExitEventArgs e)
        {
            lifetime.StopApplication();
        }

        private static void WpfMvcAppApplication_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleUnhandledException(e.Exception);
            e.Handled = true;
        }

        private static void WpfController_UnhandledException(object sender, Charites.Windows.Mvc.UnhandledExceptionEventArgs e)
        {
            HandleUnhandledException(e.Exception);
            e.Handled = true;
        }

        private static void HandleUnhandledException(Exception exc)
        {
            MessageBox.Show(exc?.ToString());
        }
    }
}
