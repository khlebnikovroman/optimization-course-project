using System;
using System.ComponentModel;
using System.Windows;


namespace OptimizatonMethods.Services
{
    public abstract class ViewModelBase : DependencyObject, INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {
            OnDispose();
        }


        public event PropertyChangedEventHandler PropertyChanged;


        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        protected virtual void OnDispose()
        {
        }

        /// Окно в котором показывается текущий ViewModel
        protected virtual void Closed()
        {
        }
    }
}
