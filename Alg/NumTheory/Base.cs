using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static partial class Alg
    {
        public static IList<int> Base(long value, int b)
        {
            if (value == 0) return new int[] { 0 };

            List<int> ret = new List<int>();

            value = Math.Abs(value);

            while (value > 0)
            {
                ret.Add((int)(value % b));
                value /= b;
            }

            return ret;
        }

        public static long Base(IList<int> value, int b)
        {
            long ret = 0;
            for (int i = value.Count-1; i >= 0; i--)
            {
                ret *= b;
                ret += value[i];
            }

            return ret;
        }
    }
}