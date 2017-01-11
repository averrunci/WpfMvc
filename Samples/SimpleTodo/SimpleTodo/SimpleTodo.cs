// Copyright (C) 2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

using Fievus.Windows.Samples.SimpleTodo.Contents;

namespace Fievus.Windows.Samples.SimpleTodo
{
    public class SimpleTodo : Application
    {
        [STAThread]
        static void Main()
        {
            new SimpleTodo().Run();
        }

        public SimpleTodo()
        {
            Startup += OnSimpleTodoStartup;
            DispatcherUnhandledException += OnSimpleTodoDispatcherUnhandledException;

            AddResourceDictionary("Resources.xaml");
        }

        private void AddResourceDictionary(string resourceFileName)
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri($"/{Assembly.GetExecutingAssembly().FullName};component/Resources/{resourceFileName}", UriKind.Relative)
            });
        }

        private void OnSimpleTodoDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
            e.Handled = true;
        }

        private void OnSimpleTodoStartup(object sender, StartupEventArgs e)
        {
            MainWindow = new Window();
            MainWindow.Style = FindResource("MainWindowStyle") as Style;
            MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MainWindow.DataContext = new MainContent();
            MainWindow.Show();
        }
    }
}
