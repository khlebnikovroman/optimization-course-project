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

        [DisplayName("Длина")]
        public double X { get; set; }

        [DisplayName("Ширина")]
        public double Y { get; set; }

        [DisplayName("Себестоимость продукта")]
        public double Z { get; set; }
    }
}
