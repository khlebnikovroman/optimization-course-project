using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace OptimizatonMethods.Models
{
    public class MathModel
    {
        private readonly Task _task;
        private readonly double _epsilon = 0.01;
        private readonly double _k = 10;
        private double _n = 2;
        private readonly double _r = 2;
        private double _step;
        private readonly double alpha;
        private readonly double beta;
        private readonly double dimSum;
        private readonly double H;
        private double lmax;
        private double lmin;
        private readonly double mu;
        private readonly double N;
        private readonly double price;
        private double smax;
        private double smin;

        public MathModel(Task task)
        {
            _task = task;
            price = (double) _task.Price;
            alpha = (double) _task.Alpha;
            beta = (double) _task.Beta;
            mu = (double) _task.Mu;
            N = (double) _task.N;
            lmin = (double) _task.Lmin;
            lmax = (double) _task.Lmax;
            smin = (double) _task.Smin;
            smax = (double) _task.Smax;
            dimSum = (double) _task.DimSum;
            H = (double) _task.H;
        }

        public int CalculationCount { get; private set; }

        public double Function(double l, double s)
        {
            return price * alpha * Math.Pow(l - s, 2) + beta * (1 / H) * Math.Pow(s + l - mu * N, 2);
        }

        private bool Conditions(double l, double s)
        {
            return l >= lmin && l <= lmax && s >= smin && s <= smax && l + s >= dimSum;
        }

        public void Calculate(out List<Point3D> points3D)
        {
            var funcMin = double.PositiveInfinity;
            _step = Math.Pow(_k, _r) * _epsilon;
            points3D = new List<Point3D>();
            var p3D = new List<Point3D>();
            List<double> values;
            Point newMin;

            newMin = SearchMinOnGrid(out p3D, out values);
            lmin = newMin.X - _step;
            smin = newMin.Y - _step;

            lmax = newMin.X + _step;
            smax = newMin.Y + _step;

            _step /= _k;
            points3D.AddRange(p3D);

            while (funcMin > values.Min())
            {
                newMin = SearchMinOnGrid(out p3D, out values);

                lmin = newMin.X - _step;
                smin = newMin.Y - _step;

                lmax = newMin.X + _step;
                smax = newMin.Y + _step;

                _step /= _k;
                funcMin = values.Min();
                points3D.AddRange(p3D);
            }
        }

        private Point SearchMinOnGrid(out List<Point3D> points3D, out List<double> values)
        {
            points3D = new List<Point3D>();

            for (var t1 = lmin; t1 <= lmax; t1 += _step)
            {
                for (var t2 = smin; t2 <= smax; t2 += _step)
                {
                    if (!Conditions(t1, t2))
                    {
                        continue;
                    }

                    CalculationCount++;
                    var value = Function(t1, t2);

                    if (value < 0)
                    {
                        MessageBox.Show($"t1 {t1} t2 {t2} Z {value}");
                    }

                    points3D.Add(new Point3D(Math.Round(t1, 2), Math.Round(t2, 2), Math.Round(value, 2)));
                }
            }

            var valuesListTemp = points3D.Select(item => item.Z).ToList();
            values = valuesListTemp;

            return new Point(points3D.Find(x => x.Z == valuesListTemp.Min()).X, points3D.Find(x => x.Z == valuesListTemp.Min()).Y);
        }
    }
}
