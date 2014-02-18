using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public partial class Alg
    {
        /// <summary>
        /// Anticlockwise oriented area.
        /// </summary>
        public static double AreaPoly2D(params double[] points)
        {
            double area = 0;
            double x1, y1, x2, y2;
            int n = points.Length / 2;
            if (n > 2)
            {
                x2 = points[2 * (n - 1)];
                y2 = points[2 * (n - 1) + 1];
                for (int i = 0; i < n; i++)
                {
                    x1 = x2;
                    y1 = y2;
                    x2 = points[2 * i];
                    y2 = points[2 * i + 1];
                    area += (y1 + y2) * (x1 - x2) / 2;
                }
            }
            return area;
        }
    }
}