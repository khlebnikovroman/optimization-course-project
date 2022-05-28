using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OptimizatonMethods.Models;

using PropertyChanged;


namespace OptimizatonMethods.MethodControls
{
    /// <summary>
    /// Interaction logic for ScanMethodControl.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class ScanMethodControl : UserControl
    {
        public ScanMethodControl(ScanMethod method)
        {
            InitializeComponent();
            Method = method;
        }
        public ScanMethod Method { get; set; }

    }
}
