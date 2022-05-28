using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using OptimizatonMethods.MethodControls;

using PropertyChanged;
using OptimizatonMethods.Models;

using WPF_MVVM_Classes;

using Point = OptimizatonMethods.Models.Point;
using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;


namespace OptimizatonMethods.ViewModels
{
    public static class MethodBuilder
    {
        public static IOptimMethod BuildOptimMethod(MethodExtended me,MathModelTest mm)
        {
            mm.buildParametersWithBound();
            if (me.MethodType == typeof(ScanMethod))
            {
                return new ScanMethod(mm.Function,mm.Conditions, mm.p1, mm.p2);
            }
            else if (me.MethodType == typeof(VariationMethod))
            {
                return new VariationMethod(mm.Function,mm.Conditions, mm.p1, mm.p2);
            }
            else
            {
                throw new ArgumentException("Указанный метод не найден", nameof(me.MethodType));
            }
        }
    }
    public static class MethodControlBuilder
    {
        public static UserControl BuildMethodControl(IOptimMethod me)
        {
            if (me.GetType() == typeof(ScanMethod))
            {
                return new ScanMethodControl(me as ScanMethod);
            }
            else if (me.GetType() == typeof(VariationMethod))
            {
                return new VariationMethodControl(me as VariationMethod);
            }
            else
            {
                throw new ArgumentException("Указанный метод не найден", nameof(me));
            }
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModelTest : ViewModelBase
    {
        #region Constructors

        public MainWindowViewModelTest()
        {
            variants = new Dictionary<string, JsonElement>();
            string jsonStr = "";
            using (StreamReader r = new StreamReader("variants.json"))
            {
                jsonStr = r.ReadToEnd();
            }
            var json = JsonDocument.Parse(jsonStr).RootElement;
            var vars = json.EnumerateArray();
            foreach (var variant in vars)
            {
                variants[variant.GetProperty("displayName").GetString()]=variant;
            }

            SelectedVariant = variants.ElementAt(15);
            SelectedMethod = Methods.First();
            OnPropertyChanged(nameof(mathModel));
        }

        #endregion
            

        #region Properties
        public Dictionary<string, JsonElement> variants { get; set; }
        public List<MethodExtended> Methods { get; set; } = new List<MethodExtended>() {
            new() {Name="Метод сканирования", Activated = true, MethodType = typeof(ScanMethod)},
            new() {Name="Метод поочередного варьирования", Activated = true,MethodType = typeof(VariationMethod)},
        };

        private KeyValuePair<string, JsonElement> _selectedVariant;
        public KeyValuePair<string, JsonElement> SelectedVariant
        {
            get
            {
                return _selectedVariant;
            }
            set
            {
                _selectedVariant=value;
                mathModel=new MathModelTest(value.Value);
                mathModel.restrictions.Bind();
                SelectedMethod = Methods.First();
                OnPropertyChanged();
            }
        }

        private MethodExtended _selectedMethod;
        public UserControl MethodControl { get; set; }
        public MethodExtended SelectedMethod
        {
            get
            {
                return _selectedMethod;
            }
            set
            {
                _selectedMethod = value;
                mathModel.Method = MethodBuilder.BuildOptimMethod(_selectedMethod, mathModel);
                MethodControl = MethodControlBuilder.BuildMethodControl(mathModel.Method);
                OnPropertyChanged();
            }
        }

        public MathModelTest mathModel { get; set; }


        #endregion


        #region Command

        private RelayCommand? _calculateCommand;
        public RelayCommand CalculateCommand
        {
            get
            {
                return _calculateCommand ??= new RelayCommand(c =>
                {
                    
                    var p = mathModel.Calculate();

                    HandyControl.Controls.MessageBox.Show($"{mathModel.desiredParameterName}: {p.Z}\n " +
                                    $"{mathModel.p1.parameter.ToString()}: {p.X}\n " +
                                    $"{mathModel.p2.parameter.ToString()}: {p.Y}", "Ответ", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }


        public RelayCommand ShowTask
        {
            get
            {
                return new RelayCommand(r =>
                {
                    HandyControl.Controls.MessageBox.Show(mathModel.Task);
                });
            }
        }

        public RelayCommand TwoDChartCommand
        {
            get
            {
                return new RelayCommand(r =>
                {
                    var test = new Chart2DWindow(mathModel);
                    test.Show();
                });
            }
        }

        public RelayCommand ThreeDChartCommand
        {
            get
            {
                return new RelayCommand(r =>
                {
                    var test = new Chart3DWindow(mathModel);
                    test.Show();
                });
            }
        }

        #endregion
    }
}
