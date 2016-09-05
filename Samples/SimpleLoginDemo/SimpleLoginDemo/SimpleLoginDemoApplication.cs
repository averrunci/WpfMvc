// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

using Fievus.Windows.Mvc;

using Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents;
using Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

namespace Fievus.Windows.Samples.SimpleLoginDemo
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
            Startup += WpfMvcViewTest_Startup;
            DispatcherUnhandledException += WpfMvcViewTest_DispatcherUnhandledException;

            AddResourceDictionary("Resources.xaml");
        }

        private void AddResourceDictionary(string resourceFileName)
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri(
                    string.Format("/{0};component/Resources/{1}", Assembly.GetAssembly(typeof(MainContent)).FullName, resourceFileName),
                    UriKind.Relative
                )
            });
        }

        private void WpfMvcViewTest_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
            e.Handled = true;
        }

        private void WpfMvcViewTest_Startup(object sender, StartupEventArgs e)
        {
            WpfController.Injector = new SimpleLoginDemoInjector();

            MainWindow = new Window();
            MainWindow.Style = FindResource("MainWindowStyle") as Style;
            MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MainWindow.DataContext = new MainContent(new LoginContent());
            MainWindow.Show();
        }
    }
}
