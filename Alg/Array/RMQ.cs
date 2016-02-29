using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static partial class Alg
    {
        /// <summary>
        /// Implements Range Minimal Query (RMQ) using a sparse table in O(1) query time and O(n log n) preprocessing time.
        /// </summary>
        public class RMQ<T>
        {
            public RMQ(IList<T> list, Comparison<T> comparison = null)
            {
                this.list = list;
                N = list.Count();

                comp = comparison;
                if (comp == null)
                {
                    Comparer<T> comparer = Comparer<T>.Default;
                    comp = (a, b) => comparer.Compare(a, b);
                }

                log = new int[N + 1];
                for (int i = 2; i <= N; i++)
                    log[i] = log[i >> 1] + 1;

                rmq = new int[log[N] + 1, N];

                for (int i = 0; i < N; ++i)
                    rmq[0, i] = i;

                for (int k = 1; (1 << k) <= N; ++k)
                {
                    for (int i = 0; i + (1 << k) <= N; i++)
                    {
                        int x = rmq[k - 1, i];
                        int y = rmq[k - 1, i + (1 << (k - 1))];
                        rmq[k, i] = comp(list[x], list[y]) <= 0 ? x : y;
                    }
                }
            }

            public int GetIndex(int start, int end)
            {
                if (start < 0) start = 0;
                if (end > N) end = N;
                if (start >= end) throw new ApplicationException(string.Format("Invalid range: [{0}, {1}]", start, end));

                int k = log[end - start];
                int x = rmq[k, start];
                int y = rmq[k, end - (1 << k)];
                return comp(list[x], list[y]) <= 0 ? x : y;
            }

            public T this[params int[] ranges]
            {
                get
                {
                    T ret = default(T);
                    bool first = true;
                    for (int i = 0; i < ranges.Length; i += 2)
                    {
                        int startRange = ranges[i];
                        int endRange = i == ranges.Length - 1 ? ranges[i] + 1 : ranges[i + 1];
                        T tmp = list[GetIndex(startRange, endRange)];
                        if (first)
                        {
                            ret = tmp;
                            first = false;
                        }
                        else if (comp(tmp, ret) < 0)
                        {
                            ret = tmp;
                        }
                    }
                    return ret;
                }
            }

            private int N;
            private IList<T> list;
            private int[,] rmq;
            private Comparison<T> comp;
            private int[] log;
        }
    }
}