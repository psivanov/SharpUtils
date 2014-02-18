using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public partial class Alg
    {
        public static long EulerTot(long n)
        {
            long tot = n;
            for (int p = 2; p * p <= n; ++p)
                if (n % p == 0)
                {
                    while (n % p == 0) n /= p;
                    tot -= tot / p;
                }

            if (n > 1) tot -= tot / n;
            return tot;
        }
    }
}
