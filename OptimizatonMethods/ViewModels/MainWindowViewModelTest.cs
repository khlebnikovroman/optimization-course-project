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
            mathModel = new MathModelTest(variants.ElementAt(0).Value);
            OnPropertyChanged(nameof(mathModel));
        }

        #endregion


        #region Variables
        private Dictionary<string, JsonElement> variants;
        private IEnumerable<Method> _allMethods;
        private IEnumerable<Task> _allTasks;
        private Task _selectedTask;
        private RelayCommand? _calculateCommand;
        private IEnumerable _dataList;
        private List<Point3D> _point3D = new();

        #endregion


        #region Properties
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

                    MessageBox.Show($"Минимальная себестоимость, у.е.: {temp.Min()}\n " +
                                    $"Длина:, м: {points3D.Find(x => x.Z == temp.Min()).X}\n " +
                                    $"Ширина, С: {points3D.Find(x => x.Z == temp.Min()).Y}");
                });
            }
        }

        public RelayCommand TwoDChartCommand
        {
            get
            {
                return new RelayCommand(r =>
                {
                    //var test = new Chart2DWindow(DataList as List<Point3D>, Task);
                    //test.Show();
                });
            }
        }

        public RelayCommand ThreeDChartCommand
        {
            get
            {
                return new RelayCommand(r =>
                {
                    //var test = new Chart3DWindow(DataList as List<Point3D>, Task);
                    //test.Show();
                });
            }
        }

        #endregion
    }
}
