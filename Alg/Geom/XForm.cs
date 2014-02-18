using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public partial class Alg
    {
        /*
          * (0, 1, 4) (x)
          * (2, 3, 5) (y)
          *           (1)
          */
        public class Matrix
        {
            private double[] coef = new double[] { 1, 0, 0, 1, 0, 0 };

            public void Mul(Matrix o)
            {
                double c0 = o.coef[0] * coef[0] + o.coef[1] * coef[2];
                double c1 = o.coef[0] * coef[1] + o.coef[1] * coef[3];
                double c2 = o.coef[2] * coef[0] + o.coef[3] * coef[2];
                double c3 = o.coef[2] * coef[1] + o.coef[3] * coef[3];
                double c4 = o.coef[0] * coef[4] + o.coef[1] * coef[5] + o.coef[4];
                double c5 = o.coef[2] * coef[4] + o.coef[3] * coef[5] + o.coef[5];

                coef[0] = c0;
                coef[1] = c1;
                coef[2] = c2;
                coef[3] = c3;
                coef[4] = c4;
                coef[5] = c5;
            }

            public void Scale(double x, double y)
            {
                Matrix tmp = new Matrix();
                tmp.coef[0] = x;
                tmp.coef[3] = y;
                Mul(tmp);
            }
            public void Translate(double x, double y)
            {
                coef[4] += x;
                coef[5] += y;
            }
            public void Rotate(double angle)
            {
                Matrix tmp = new Matrix();
                double cos = Math.Cos(angle);
                double sin = Math.Sin(angle);
                tmp.coef[0] = tmp.coef[3] = cos;
                tmp.coef[1] = -(tmp.coef[2] = sin);
                Mul(tmp);
            }
            public void Rotate(double fromx, double fromy, double tox, double toy)
            {
                double angle = Math.Atan2(toy, tox);
                angle -= Math.Atan2(fromy, fromx);

                double fromL = Math.Sqrt(fromx * fromx + fromy * fromy);
                double toL = Math.Sqrt(tox * tox + toy * toy);
                double scale = toL / fromL;

                Rotate(angle);
                Scale(scale, scale);
            }
            public double[] XForm(double[] vec)
            {
                double[] ret = new double[2];
                ret[0] = coef[4];
                ret[0] += coef[0] * vec[0];
                ret[0] += coef[1] * vec[1];

                ret[1] = coef[5];
                ret[1] += coef[2] * vec[0];
                ret[1] += coef[3] * vec[1];

                return ret;
            }
        }
    }
}