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

        [DisplayName("Температура в змеевике")]
        public double X { get; set; }

        [DisplayName("Температура в диффузоре")]
        public double Y { get; set; }

        [DisplayName("Себестоимость продукта")]
        public double Z { get; set; }
    }
}
