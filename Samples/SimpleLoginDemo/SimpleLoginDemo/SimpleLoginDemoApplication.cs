// Copyright (C) 2018-2020 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;
using Charites.Windows.Mvc;
using Microsoft.Extensions.Hosting;

namespace Charites.Windows.Samples.SimpleLoginDemo
{
    public class SimpleLoginDemoApplication : Application
    {
        private readonly IHostApplicationLifetime lifetime;

        public SimpleLoginDemoApplication(IHostApplicationLifetime lifetime, IServiceProvider services)
        {
            this.lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));

            Startup += SimpleLoginDemoApplication_Startup;
            Exit += SimpleLoginDemoApplication_Exit;
            DispatcherUnhandledException += SimpleLoginDemoApplication_DispatcherUnhandledException;

            WpfController.ControllerFactory = new SimpleLoginDemoControllerFactory(services);

            AddResourceDictionary("Resources.xaml");
        }

        private void AddResourceDictionary(string resourceFileName)
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri($"/{Assembly.GetAssembly(typeof(MainContent)).FullName};component/Resources/{resourceFileName}", UriKind.Relative)
            });
        }

        private void SimpleLoginDemoApplication_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
            e.Handled = true;
        }

        private void SimpleLoginDemoApplication_Startup(object sender, StartupEventArgs e)
        {
            MainWindow = new Window
            {
                Style = FindResource("MainWindowStyle") as Style,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                DataContext = new MainContent(new LoginContent())
            };
            MainWindow.Show();
        }

        private void SimpleLoginDemoApplication_Exit(object sender, ExitEventArgs e)
        {
            lifetime.StopApplication();
        }
    }
}
