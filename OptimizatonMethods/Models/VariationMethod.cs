using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xaml.Schema;

using PropertyChanged;


namespace OptimizatonMethods.Models
{
    [AddINotifyPropertyChangedInterface]
    public class VariationMethod : IOptimMethod
    {
        public VariationMethod(Func<double, double, double> f,Func<double,double,bool> cond, ParameterWithBounds x1, ParameterWithBounds x2) : this(f,cond,x1.min, x1.max, x2.min, x2.max)
        {

        }
        public VariationMethod(Func<double, double, double> f, Func<double, double, bool> cond,double xmin, double xmax, double ymin, double ymax)
        {
            this.Func=f;
            Conditions = cond;
            func = Func;
            this.xmin = xmin;
            this.xmax = xmax;
            this.ymin = ymin;
            this.ymax = ymax;
            this.StartPoint = new Point(1, 1);
            Point3Ds = new ObservableCollection<Point3D>();
        }
        private double xmin, xmax, ymin, ymax;
        public Point StartPoint { get; set; }
        public int CountOfCalc { get; set; } = 150;
        public Func<double, double, double> Func { get; init; }
        private double epsilon = 0.001;
        private Func<double, double, double> func;

        public ObservableCollection<Point3D> Point3Ds { get; set; }
        public Func<double, double, bool> Conditions { get; set; }

        public Point3D FindOptimalParametersMin()
        {
            Point3Ds = new ObservableCollection<Point3D>();
            var x = StartPoint.X;
            var y = StartPoint.Y;
            Point3Ds.Add(new Point3D(StartPoint,func(x,y)));
            var step = (xmax - xmin + ymax - ymin) / 100;

            for (int i = 0; i < CountOfCalc; i++)
            {
                if (AdditionalMath.dX(x, y, func) > 0)
                {
                    x = dyhX(x,x-step,x,y);
                }
                else
                {
                    x = dyhX(x, x + step, x, y);
                }
                if (x < xmin)
                {
                    x = xmin;
                }
                if (x > xmax)
                {
                    x = xmax;
                }
                Point3Ds.Add(new Point3D(x, y, func(x, y)));
                if (AdditionalMath.dY(x, y, func) > 0)
                {
                    y = dyhY(y, y - step, x, y);
                }
                else
                {
                    y = dyhY(y, y + step, x, y);
                }
                if (y < ymin)
                {
                    y = ymin;
                }
                if (y > ymax)
                {
                    y = ymax;
                }

                step *= 1;
                Point3Ds.Add(new Point3D(x,y,func(x,y)));
            }

            return Point3Ds.Last();
        }
        
        private double dyhX(double a, double b, double x, double y)
        {
            while(Math.Abs(func(b,y) - func(a,y)) > epsilon)
            {
                var mid = (a + b) / 2;
                if (func(a,y) * func(b, y) < 0)
                {
                    b = mid;
                }
                else
                {
                    a = mid;
                }
            }
            return (b + a) / 2;
        }
        private double dyhY(double a, double b, double x, double y)
        {
            while (Math.Abs(func(x, b) - func(x, a)) > epsilon)
            {
                var mid = (a + b) / 2;
                if (func(x, a) * func(x, b) < 0)
                {
                    b = mid;
                }
                else
                {
                    a = mid;
                }
            }
            return (b + a) / 2;
        }

        public Point3D FindOptimalParametersMax()
        {
            func = (x1, x2) => -Func(x1,x2);    //инвертируем функцию
            var p = FindOptimalParametersMin(); 
            func = Func; // возвращаем обратно
            return p;
        }

        
    }
}
