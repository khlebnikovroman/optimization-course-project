using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace OptimizatonMethods
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                e.Column.Header = descriptor.DisplayName ?? descriptor.Name;
            }
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
           // FontSize = (ActualHeight + ActualHeight / ActualWidth * ActualWidth) / 75;
        }
    }
}
