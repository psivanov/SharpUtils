using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public partial class Alg
    {
        /// <summary>
        /// Convex hull for a set of 2D points.
        /// </summary>
        public static List<double[]> ConvexHull(List<double[]> points)
        {
            List<double[]> sorted = points.ToList();
            sorted.Sort((p1, p2) =>
            {
                int result = p1[0].CompareTo(p2[0]);
                if (result == 0)
                    result = p1[1].CompareTo(p2[1]);

                return result;
            });

            int N = sorted.Count;

            if (N == 1)
                return sorted;

            List<double[]> L = new List<double[]>(N);
            List<double[]> U = new List<double[]>(N);
            
            int count;
            double area;

            for (int i = 0; i < N; i++)
            {
                count = L.Count;
                while (count > 1)
                {
                    area = Area2Poly2D(L[count - 2], L[count - 1], sorted[i]);
                    if (area <= 0)
                        L.RemoveAt(--count);
                    else
                        break;
                }
                L.Add(sorted[i]);
            }
            L.RemoveAt(L.Count - 1);

            for (int i = N - 1; i >= 0; i--)
            {
                count = U.Count;
                while (count > 1)
                {
                    area = Area2Poly2D(U[count - 2], U[count - 1], sorted[i]);
                    if (area <= 0)
                        U.RemoveAt(--count);
                    else
                        break;
                }
                U.Add(sorted[i]);
            }
            U.RemoveAt(U.Count - 1);

            return L.Concat(U).ToList();
        }
    }
}