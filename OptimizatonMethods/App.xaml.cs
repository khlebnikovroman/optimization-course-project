﻿using OptimizatonMethods.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OptimizatonMethods
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            var mainWindow = new MainWindow { DataContext = new MainWindowViewModel() };
            mainWindow.Show();
        }
    }
}
