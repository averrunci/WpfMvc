using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace $safeprojectname$
{
    public class $safeitemrootname$ : Application
    {
        [STAThread]
        static void Main()
        {
            new $safeitemrootname$().Run();
        }

        public $safeitemrootname$()
        {
            Startup += WpfMvcViewTest_Startup;
            DispatcherUnhandledException += WpfMvcViewTest_DispatcherUnhandledException;

            AddResourceDictionary("Resources.xaml");
        }

        private void AddResourceDictionary(string resourceFileName)
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri($"/{Assembly.GetExecutingAssembly().FullName};component/Resources/{resourceFileName}", UriKind.Relative)
            });
        }

        private void WpfMvcViewTest_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
            e.Handled = true;
        }

        private void WpfMvcViewTest_Startup(object sender, StartupEventArgs e)
        {
            MainWindow = new Window();
            MainWindow.Style = FindResource("MainWindowStyle") as Style;
            MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MainWindow.DataContext = new MainContent();
            MainWindow.Show();
        }
    }
}
