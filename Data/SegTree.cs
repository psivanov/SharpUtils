using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Data
{
    public class SegTree<T>
    {
        public SegTree(int N, Func<T, T, T> assoc, IEnumerable<T> data = null)
        {
            this.N = N;
            this.assoc = assoc;
            D = 1;
            for (int tmp = 1; tmp < N; tmp <<= 1) ++D;
            theTree = new T[N, D];

            if (data != null)
            {
                int ind = 0;
                foreach (T item in data.Take(N))
                    theTree[ind++, 0] = item;
            }

            for (int d = 1; d < D; d++)
            {
                for (int i = 0; i < N; i += (1 << d))
                {
                    if (i + (1 << (d - 1)) < N)
                        theTree[i, d] = assoc(theTree[i, d - 1], theTree[i + (1 << (d - 1)), d - 1]);
                    else
                        theTree[i, d] = theTree[i, d - 1];
                }
            }
        }

        public T Get(int start, int end)
        {
            if (start < 0) start = 0;
            if (end > N) end = N;
            if (start >= end) throw new ApplicationException(string.Format("Invalid range: [{0}, {1}]", start, end));

            bool first = true;
            int d = 0;
            T ret = default(T);

            while (start < end)
            {
                for (; (start & (1 << d)) == 0 && d < D - 1; ++d) ;
                for (; start + (1 << d) >= end && d > 0; --d) ;

                if (first)
                {
                    ret = theTree[start, d];
                    first = false;
                }
                else
                {
                    ret = assoc(ret, theTree[start, d]);
                }
                start += (1 << d);
            }

            return ret;
        }

        public void Set(int index, T value)
        {
            if (index < 0 || index >= N) throw new ApplicationException(string.Format("Index out of range: {0}", index));
            theTree[index, 0] = value;
            for (int d = 1; d < D; d++)
            {
                int x = index - (index & ((1 << d) - 1));
                if (x + (1 << (d - 1)) < N)
                    theTree[x, d] = assoc(theTree[x, d - 1], theTree[x + (1 << (d - 1)), d - 1]);
                else
                    theTree[x, d] = theTree[x, d - 1];
            }
        }

        public T this[params int[] ranges]
        {
            set
            {
                for (int i = 0; i < ranges.Length; i += 2)
                {
                    int startRange = ranges[i];
                    int endRange = i == ranges.Length - 1 ? ranges[i] + 1 : ranges[i + 1];
                    for (int pos = startRange; pos < endRange; pos++)
                    {
                        Set(pos, value);
                    }
                }
            }
            get
            {
                T ret = default(T);
                bool first = true;
                for (int i = 0; i < ranges.Length; i += 2)
                {
                    int startRange = ranges[i];
                    int endRange = i == ranges.Length - 1 ? ranges[i] + 1 : ranges[i + 1];
                    if (first)
                    {
                        ret = Get(startRange, endRange);
                        first = false;
                    }
                    else
                    {
                        ret = assoc(ret, Get(startRange, endRange));
                    }
                }
                return ret;
            }
        }

        private Func<T, T, T> assoc;
        private T[,] theTree;
        private int N;
        private int D;
    }
}
