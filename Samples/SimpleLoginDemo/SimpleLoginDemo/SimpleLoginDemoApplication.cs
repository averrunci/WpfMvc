// Copyright (C) 2018 Fievus
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

namespace Charites.Windows.Samples.SimpleLoginDemo
{
    public class SimpleLoginDemo : Application
    {
        [STAThread]
        static void Main()
        {
            new SimpleLoginDemo().Run();
        }

        public SimpleLoginDemo()
        {
            Startup += SimpleLoginDemo_Startup;
            DispatcherUnhandledException += SimpleLoginDemo_DispatcherUnhandledException;

            AddResourceDictionary("Resources.xaml");
        }

        private void AddResourceDictionary(string resourceFileName)
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri($"/{Assembly.GetAssembly(typeof(MainContent)).FullName};component/Resources/{resourceFileName}", UriKind.Relative)
            });
        }

        private void SimpleLoginDemo_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
            e.Handled = true;
        }

        private void SimpleLoginDemo_Startup(object sender, StartupEventArgs e)
        {
            WpfController.ControllerFactory = new SimpleLoginDemoControllerFactory();

            MainWindow = new Window
            {
                Style = FindResource("MainWindowStyle") as Style,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                DataContext = new MainContent(new LoginContent())
            };
            MainWindow.Show();
        }
    }
}
