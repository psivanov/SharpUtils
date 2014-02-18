using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static partial class Alg
    {
        //Maximum flow in O(V*V*E)
        public class Dinitz
        {
            public const long INF = long.MaxValue;

            public Dinitz(int V, int source, int sink)
            {
                this.V = V; this.source = source; this.sink = sink;
                Edges = new List<Edge>();
                Adj = new List<int>[V];
                Dist = new int[V];
                Q = new int[V];
                Ptr = new int[V];
            }
            public void AddEdge(int a, int b, long cap)
            {
                Edge e1 = new Edge { a = a, b = b, cap = cap, flow = 0 };
                Edge e2 = new Edge { a = b, b = a, cap = 0, flow = 0 };
                if (Adj[a] == null) Adj[a] = new List<int>();
                Adj[a].Add(Edges.Count);
                Edges.Add(e1);
                if (Adj[b] == null) Adj[b] = new List<int>();
                Adj[b].Add(Edges.Count);
                Edges.Add(e2);
            }
            public long GetFlow()
            {
                long flow = 0;
                while (BFS())
                {
                    for (int i = 0; i < V; ++i) Ptr[i] = 0;
                    long pushed;
                    while ((pushed = DFS(source, INF)) > 0)
                        flow += pushed;
                }
                return flow;
            }

            private bool BFS()
            {
                int qh = 0, qt = 0;
                for (int i = 0; i < V; ++i) Dist[i] = -1;
                Dist[source] = 0;
                Q[qt++] = source;
                while (qh < qt && Dist[sink] == -1)
                {
                    int v = Q[qh++];
                    foreach (int eId in Adj[v])
                    {
                        int w = Edges[eId].b;
                        if (Dist[w] == -1 && Edges[eId].flow < Edges[eId].cap)
                        {
                            Q[qt++] = w;
                            Dist[w] = Dist[v] + 1;
                        }
                    }
                }
                return Dist[sink] != -1;
            }
            private long DFS(int v, long flow)
            {
                if (flow == 0) return 0;
                if (v == sink) return flow;
                for (; Ptr[v] < Adj[v].Count; ++Ptr[v])
                {
                    int eId = Adj[v][Ptr[v]];
                    int w = Edges[eId].b;
                    if (Dist[w] != Dist[v] + 1) continue;
                    long pushed = DFS(w, Math.Min(flow, Edges[eId].cap - Edges[eId].flow));
                    if (pushed > 0)
                    {
                        Edges[eId].flow += pushed;
                        Edges[eId ^ 1].flow -= pushed;
                        return pushed;
                    }
                }
                return 0;
            }

            private class Edge { public int a; public int b; public long cap; public long flow; };
            private int V, source, sink;
            private List<Edge> Edges;
            private List<int>[] Adj;
            private int[] Dist, Q;
            private int[] Ptr;
        }
    }
}