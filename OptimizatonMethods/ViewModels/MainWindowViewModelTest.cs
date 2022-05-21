using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using PropertyChanged;
using OptimizatonMethods.Models;

using WPF_MVVM_Classes;

using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;


namespace OptimizatonMethods.ViewModels
{
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

            SelectedVariant = variants.ElementAt(0);
            //mathModel = new MathModelTest(variants.ElementAt(0).Value);
            OnPropertyChanged(nameof(mathModel));
        }

        #endregion


        #region Variables

        public Dictionary<string, JsonElement> variants { get; set; }
        private IEnumerable<Method> _allMethods;
        private IEnumerable<Task> _allTasks;
        private Task _selectedTask;
        private RelayCommand? _calculateCommand;
        private IEnumerable _dataList;
        private List<Point3D> _point3D = new();

        #endregion


        #region Properties

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
                    OnPropertyChanged();
                }
            }
        public MathModelTest mathModel { get; set; }


        public IEnumerable DataList
        {
            get
            {
                return _dataList;
            }
            set
            {
                _dataList = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Command

        public RelayCommand CalculateCommand
        {
            get
            {
                return _calculateCommand ??= new RelayCommand(c =>
                {
                    
                    mathModel.Calculate(out var points3D);
                    DataList = points3D;

                    var temp = new List<double>();

                    foreach (var item in points3D)
                    {
                        temp.Add(item.Z);
                    }

                    double x = 0;
                    double y = 0;
                    double z = 0;

                    if (mathModel.minMax == MathModelTest.MinMax.Max)
                    {
                        z = temp.Max();
                        x = points3D.Find(x => x.Z == z).X;
                        y = points3D.Find(x => x.Z == z).Y;
                    }
                    else if (mathModel.minMax == MathModelTest.MinMax.Min)
                    {
                        z = temp.Min();
                        x = points3D.Find(x => x.Z == z).X;
                        y = points3D.Find(x => x.Z == z).Y;
                    }

                    
                    HandyControl.Controls.MessageBox.Show($"{mathModel.desiredParameterName}: {z}\n " +
                                    $"{mathModel.p1.parameter.ToString()}: {x}\n " +
                                    $"{mathModel.p2.parameter.ToString()}: {y}", "Ответ", MessageBoxButton.OK, MessageBoxImage.Information);
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
