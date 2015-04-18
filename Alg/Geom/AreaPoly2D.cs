using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public partial class Alg
    {
        /// <summary>
        /// Anticlockwise oriented area multiplied by 2.
        /// Points are packed in array of pairs.
        /// </summary>
        public static double Area2Poly2D(params double[][] points)
        {
            double area = 0;
            double x1, y1, x2, y2;
            int n = points.Length;
            if (n > 2)
            {
                x2 = points[n-1][0];
                y2 = points[n-1][1];
                for (int i = 0; i < n; i++)
                {
                    x1 = x2;
                    y1 = y2;
                    x2 = points[i][0];
                    y2 = points[i][1];
                    area += (y1 + y2) * (x1 - x2);
                }
            }
            return area;
        }

        /// <summary>
        /// Anticlockwise oriented area multiplied by 2.
        /// Points are packed in a single array.
        /// </summary>
        public static double Area2Poly2D(params double[] points)
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
                    area += (y1 + y2) * (x1 - x2);
                }
            }
            return area;
        }
    }
}