using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static partial class Alg
    {
        public static IList<long[]> Factors(long n)
        {
            List<long[]> ret = new List<long[]>();
            long[] factor = new long[] { 1L, 1L };
            for (long p = 2L; p * p <= n; ++p)
            {
                if (n % p == 0)
                {
                    n /= p;
                    factor = new long[] { p, 1L };
                    ret.Add(factor);
                    while (n % p == 0)
                    {
                        n /= p;
                        factor[1]++;
                    }
                }
            }
            if (n > 1)
            {
                if (n == factor[0]) factor[1]++;
                else ret.Add(new long[] { n, 1L });
            }
            return ret;
        }

        public static bool IsPrime(long n)
        {
            for (long p = 2L; p * p <= n; ++p)
                if (n % p == 0) return false;
            return true;
        }
    }
}
