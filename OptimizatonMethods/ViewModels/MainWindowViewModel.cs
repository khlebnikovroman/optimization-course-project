using System;
using OptimizatonMethods.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using OptimizatonMethods.Services;
using WPF_MVVM_Classes;
using ViewModelBase = OptimizatonMethods.Services.ViewModelBase;

namespace OptimizatonMethods.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Variables
        private IEnumerable<Method> _allMethods;
        private IEnumerable<Task> _allTasks;
        private Task _selectedTask;
        private RelayCommand? _calculateCommand;
        private IEnumerable _dataList;
        private List<Point3D> _point3D = new();
        #endregion

        #region Constructors
        public MainWindowViewModel()
        {
            _task = new Task(){Alpha = 1, Beta = 1, Mu = 1, H = 9, N=10, DimSum = 11, Lmin = 1, Lmax = 15, Smin = 1, Smax = 12, Price = 100, Step = 0.1} ;
        }
        #endregion

        #region Properties
        public IEnumerable<Method> AllMethods
        {
            get => _allMethods;
            set
            {
                _allMethods = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Task> AllTasks
        {
            get => _allTasks;
            set
            {
                _allTasks = value;
                OnPropertyChanged();
            }
        }

        private Task _task;

        public Task Task
        {
            get
            {
                return _task;
            }
            set
            {
                _task = value;
            }
        }

        public IEnumerable DataList
        {
            get => _dataList;
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
                    var calc = new MathModel(Task);
                    calc.Calculate(out var points3D);
                    DataList = points3D;

                    var temp = new List<double>();

                    foreach (var item in points3D)
                    {
                        temp.Add(item.Z);
                    }

                    MessageBox.Show($"Минимальная себестоимость, у.е.: {temp.Min()}\n " +
                                    $"Температура в змеевике Т1, С: {points3D.Find(x => x.Z == temp.Min()).X}\n " +
                                    $"Температура в диффузоре Т2, С: {points3D.Find(x => x.Z == temp.Min()).Y}");

                });
            }
        }

        public RelayCommand TwoDChartCommand
        {
            get
            {
                return new RelayCommand(r =>
                {
                    var test = new Chart2DWindow(DataList as List<Point3D>, Task);
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
                    var test = new Chart3DWindow(DataList as List<Point3D>, Task);
                    test.Show();
                });
            }
        }



        #endregion


    }
}
