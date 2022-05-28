using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizatonMethods.Models
{

    public interface IOptimMethod
    {
        public Func<double,double,bool> Conditions { get; set; }
        public Func<double,double,double> Func { get; init; }
        public Point3D FindOptimalParametersMin();
        public Point3D FindOptimalParametersMax();
        public ObservableCollection<Point3D> Point3Ds { get; set; }

        public Point3D LastPoint3D
        {
            get
            {
                return Point3Ds.Last();
            }
        }
    }
}
