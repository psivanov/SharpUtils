using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public partial class Alg
    {
        /// <summary>
        /// Anticlockwise oriented distance from point to line.
        /// (0,1) to (-1,0),(1,0) is +1
        /// </summary>
        public static double DistPtLine2D(double[] pt, double[] l1, double[] l2)
        {
            double dx = l2[0] - l1[0];
            double dy = l2[1] - l1[0];

            return (dx * (pt[1] - l1[1]) - dy * (pt[0] - l1[0])) / Math.Sqrt(dx * dx + dy * dy);
        }
    }
}