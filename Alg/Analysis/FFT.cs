using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Data;

namespace Utils
{
    public static partial class Alg
    {
        public static void FFT(Complex[] a)
        {
            FFT(a, false);
        }

        public static void FFT(Complex[] a, bool invert)
        {
            int n = (int)a.Length;
            if (n == 1) return;

            Complex[] a0 = new Complex[n / 2];
            Complex[] a1 = new Complex[n / 2];
            for (int i = 0, j = 0; i < n; i += 2, ++j)
            {
                a0[j] = a[i];
                a1[j] = a[i + 1];
            }
            FFT(a0, invert);
            FFT(a1, invert);

            double ang = 2 * Math.PI / n * (invert ? -1 : 1);
            Complex w = 1;
            Complex wn = Complex.FromPolar(1, ang);
            for (int i = 0; i < n / 2; ++i)
            {
                a[i] = a0[i] + w * a1[i];
                a[i + n / 2] = a0[i] - w * a1[i];
                if (invert)
                {
                    a[i] /= 2; a[i + n / 2] /= 2;
                }
                w *= wn;
            }
        }
    }
}