using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static partial class Alg
    {
        public static void ZFunc<T>(IList<T> s, int n, int[] Z)
        {
            if (n > 0)
            {
                Z[0] = n;
                int L = -1;
                int R = -1;
                for (int i = 1; i < n; ++i)
                {
                    if (i <= R && Z[i - L] < R - i + 1) Z[i] = Z[i - L];
                    else
                    {
                        L = i;
                        R = Math.Max(R, i);
                        while (R < n && s[R].Equals(s[R - L])) ++R;
                        Z[i] = R-- - L;
                    }
                }
            }
        }

        public static void ZFunc(string s, int n, int[] Z)
        {
            if (n > 0)
            {
                Z[0] = n;
                int L = -1;
                int R = -1;
                for (int i = 1; i < n; ++i)
                {
                    if (i <= R && Z[i - L] < R - i + 1) Z[i] = Z[i - L];
                    else
                    {
                        L = i;
                        R = Math.Max(R, i);
                        while (R < n && s[R].Equals(s[R - L])) ++R;
                        Z[i] = R-- - L;
                    }
                }
            }
        }
    }
}
