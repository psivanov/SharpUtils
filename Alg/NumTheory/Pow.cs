using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public partial class Alg
    {
        public static long Pow(long a, long n)
        {
            long res = 1L;
            while (n > 0)
            {
                if ((n & 1) != 0)
                    res *= a;
                a *= a;
                n >>= 1;
            }
            return res;
        }
        public static long Pow(long a, long n, long mod)
        {
            long res = 1L;
            while (n > 0)
            {
                if ((n & 1) != 0)
                    res = (res * a) % mod;
                a = (a * a) % mod;
                n >>= 1;
            }
            return res;
        }
    }
}