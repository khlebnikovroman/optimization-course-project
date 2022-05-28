using System;
using System.Collections.Generic;
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
    /// Interaction logic for VariationMethodControl.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class VariationMethodControl : UserControl
    {
        public VariationMethodControl(VariationMethod method)
        {
            InitializeComponent();
            Method = method;
        }

        public VariationMethod Method { get; set; }

    }
}
