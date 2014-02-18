using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public partial class Alg
    {
        public static long GCD(long a, long b)
        {
            long tmp;
            while (b != 0)
            {
                a %= b;
                tmp = a; a = b; b = tmp;
            }
            return a;
        }
    }
}
