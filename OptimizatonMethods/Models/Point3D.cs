using System.ComponentModel;


namespace OptimizatonMethods.Models
{
    public class Point3D
    {
        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3D(Point point, double z):this(point.X,point.Y,z)
        {
            
        }
        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }
    }
}
