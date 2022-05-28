using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizatonMethods.Models
{
    public static class AdditionalMath
    {
        public static double dY(double x, double y, Func<double,double,double> func)
        {
            var epsilon = 1E-12;
            var dx = 1.0;
            var f1 = func(x, y + dx);
            var f2 = func(x, y);
            var dy = f1 - f2;
            var deriv_p = dy / dx;
            while (true)
            {
                dx = 0.5 * dx;
                dy = func(x, y + dx) - func(x, y);
                var deriv_c = dy / dx;
                if (Math.Abs(deriv_c - deriv_p) < epsilon)
                {
                    return deriv_c;
                }
                deriv_p = deriv_c;
            }
        }
        public static double dX(double x, double y, Func<double, double, double> func)
        {
            var epsilon = 1E-12;
            var dx = 1.0;
            var dy = func(x+dx, y ) - func(x, y);
            var deriv_p = dy / dx;
            while (true)
            {
                dx = 0.5 * dx;
                dy = func(x + dx, y) - func(x, y);
                var deriv_c = dy / dx;
                if (Math.Abs(deriv_c - deriv_p) < epsilon)
                {
                    return deriv_c;
                }
                deriv_p = deriv_c;
            }
        }
    }
}
