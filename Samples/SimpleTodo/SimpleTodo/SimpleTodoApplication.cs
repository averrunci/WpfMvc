// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Charites.Windows.Samples.SimpleTodo.Contents;
using Microsoft.Extensions.Hosting;

namespace Charites.Windows.Samples.SimpleTodo;

public class SimpleTodoApplication : Application
{
    private readonly IHostApplicationLifetime lifetime;

    public SimpleTodoApplication(IHostApplicationLifetime lifetime)
    {
        this.lifetime = lifetime;

        Startup += OnSimpleTodoApplicationStartup;
        Exit += OnSimpleTodoApplicationExit;
        DispatcherUnhandledException += OnSimpleTodoApplicationDispatcherUnhandledException;

        AddResourceDictionary("Resources.xaml");
    }

    private void AddResourceDictionary(string resourceFileName)
    {
        Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri($"/{Assembly.GetExecutingAssembly().FullName};component/Resources/{resourceFileName}", UriKind.Relative)
        });
    }

    private void OnSimpleTodoApplicationDispatcherUnhandledException(object? sender, DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show(e.Exception.ToString());
        e.Handled = true;
    }

    private void OnSimpleTodoApplicationStartup(object? sender, StartupEventArgs e)
    {
        MainWindow = new Window
        {
            Style = FindResource("MainWindowStyle") as Style,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            DataContext = new MainContent()
        };
        MainWindow.Show();
    }

    private void OnSimpleTodoApplicationExit(object? sender, ExitEventArgs e)
    {
        lifetime.StopApplication();
    }
}