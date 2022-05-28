using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Schema;

using HandyControl.Controls;

using PropertyChanged;


namespace OptimizatonMethods.Models
{
    [AddINotifyPropertyChangedInterface]
    public class ScanMethod : IOptimMethod
    {

        public ScanMethod(Func<double, double, double> f, Func<double, double, bool> cond, ParameterWithBounds x1, ParameterWithBounds x2) : this(f,cond, x1.min, x1.max, x2.min, x2.max)
        {

        }
        public ScanMethod(Func<double, double, double> f, Func<double, double, bool> cond, double xmin, double xmax, double ymin, double ymax)
        {
            this.Func = f;
            func = Func;
            Conditions = cond;
            this.xmin = xmin;
            this.xmax = xmax;
            this.ymin = ymin;
            this.ymax = ymax;
            Point3Ds = new ObservableCollection<Point3D>();
        }

        public double Epsilon { get; set; } = 0.01;
        private readonly double xmin, xmax, ymin, ymax;
        public ObservableCollection<Point3D> Point3Ds { get; set; }
        public Func<double, double, double> Func { get; init; }
        public Func<double, double, bool> Conditions { get; set ; }

        private Func<double, double, double> func;
        public Point3D FindOptimalParametersMin()
        {
            Point3Ds = new ObservableCollection<Point3D>();
            return findOptimalParameter(null, xmin, xmax, ymin, ymax);
        }

        private Point3D findOptimalParameter(double? prevFunc,double xmin, double xmax, double ymin, double ymax)
        {
            int countOfX = 30, countOfY = 30;
            var stepX = (xmax - xmin)/ countOfX;
            var stepY = (ymax - ymin)/ countOfY;
            Point3D[,] points = new Point3D[countOfX,countOfY];

            for (int i = 0; i < countOfX; i++)
            {
                for (int j = 0; j < countOfY; j++)
                {
                    var p = new Point(xmin + stepX * (i + 1), ymin + stepY * (j + 1));
                    var p3d = new Point3D(p, func(p.X, p.Y));
                    if (Conditions(p.X, p.Y))
                    {
                        Point3Ds.Add(p3d);
                        points[i, j] = p3d;
                    }
                    else
                    {
                        p3d.Z = double.PositiveInfinity;
                        points[i, j] = p3d;
                    }
                }
            }

            IEnumerable<Point3D> all = points.Cast<Point3D>();
            var minPoint = all.MinBy(k => k.Z);

            if (prevFunc == null)
            {
                prevFunc = minPoint.Z + Epsilon * 10;
            }
            var e = Math.Abs((double) prevFunc - minPoint.Z);

            if (e < Epsilon)
            {
                return minPoint;
            }
            else
            {
                xmin = minPoint.X - stepX*2;
                xmax = minPoint.X + stepX*2;
                ymin = minPoint.Y - stepY*2;
                ymax = minPoint.Y + stepY*2;

                return findOptimalParameter(minPoint.Z, xmin, xmax, ymin, ymax);
            }

        }
        public Point3D FindOptimalParametersMax()
        {
            func = (x1, x2) => -Func(x1, x2); //инвертируем функцию
            var p = FindOptimalParametersMin();
            func = Func; // возвращаем обратно
            return p;
        }

       
    }
}
