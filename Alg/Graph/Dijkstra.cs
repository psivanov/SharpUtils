using System.Collections.Generic;
using Utils.Data;

namespace Utils
{
    public static partial class Alg
    {
        public static void Dijkstra(Dictionary<int, int>[] adj, int source, out int[] dist, out int[] pred)
        {
            int inf = int.MaxValue;
            int N = adj.Length;
            dist = new int[N];
            pred = new int[N];
            for (int i = 0; i < N; i++)
                dist[i] = inf;
            dist[source] = 0;

            Heap<int, int> heap = new Heap<int, int>(N, true);
            heap.Push(source, 0);

            while (!heap.IsEmpty())
            {
                int u = heap.PeekData();
                if (dist[u] != heap.Pop().Priority) continue;
                foreach (var tuple in adj[u])
                {
                    int v = tuple.Key;
                    int uvWeight = tuple.Value;
                    if (dist[v] > dist[u] + uvWeight)
                    {
                        dist[v] = dist[u] + uvWeight;
                        pred[v] = u;
                        heap.Push(v, dist[v]);
                    }
                }
            }
        }
    }
}
