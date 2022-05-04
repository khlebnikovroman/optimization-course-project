using System.Windows;

using OptimizatonMethods.ViewModels;


namespace OptimizatonMethods
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = new MainWindow {DataContext = new MainWindowViewModel(),};
            mainWindow.Show();
        }
    }
}
