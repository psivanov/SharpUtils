using System;

namespace Utils
{
    public static partial class Alg
    {
        public static int[,] FloydWarshal(int[,] graph, int inf)
        {
            //d.length == d[0].length; d[i,j]==inf i-th and j-th node are not connected 
            int[,] d = new int[graph.GetLength(0), graph.GetLength(1)];
            Array.Copy(graph, d, graph.Length);

            double n = d.GetLength(0);
            for (int i = 0; i < n; i++)
                d[i, i] = 0;
            int tmp;
            for (int k = 0; k < n; k++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        tmp = (d[i, k] == inf || d[k, j] == inf) ? inf : d[i, k] + d[k, j];
                        d[i, j] = Math.Min(d[i, j], tmp);
                    }
            return d;
        }
    }
}
