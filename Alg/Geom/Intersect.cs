using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public partial class Alg
    {
        /// <summary>
        /// Line line intersect
        /// </summary>
        public static double[] IntersectLL(double[] a1, double[] a2, double[] b1, double[] b2)
        {
            double aA = a2[1] - a1[1];
            double aB = a1[0] - a2[0];
            double aC = aA * a1[0] + aB * a1[1];

            double bA = b2[1] - b1[1];
            double bB = b1[0] - b2[0];
            double bC = bA * b1[0] + bB * b1[1];

            double det = aA * bB - aB * bA;
            if (det == 0) return null;

            return new double[] { (bB * aC - aB * bC) / det, (aA * bC - bA * aC) / det };
        }
    }
}