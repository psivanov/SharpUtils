using System;

namespace Utils
{
    public static partial class Alg
    {
        public static bool NextPermutation<T>(T[] arr) where T : IComparable<T>
        {
            return NextPermutation(arr, (a, b) => a.CompareTo(b));
        }

        public static bool NextPermutation<T>(T[] arr, Comparison<T> comp)
        {
            return NextPermutation(arr, 0, arr.Length, comp);
        }

        public static bool NextPermutation<T>(T[] arr, int start, int count, Comparison<T> comp)
        {
            if (count < 2) return false;

            for (int i = start + count - 2; i >= start; --i)
            {
                if (comp(arr[i], arr[i + 1]) < 0)
                {
                    for (int j = start + count - 1; ; --j)
                    {
                        if (comp(arr[i], arr[j]) < 0)
                        {
                            T tmp = arr[i]; arr[i] = arr[j]; arr[j] = tmp;
                            Array.Reverse(arr, i + 1, count - (i + 1 - start));
                            return true;
                        }
                    }
                }
            }

            Array.Reverse(arr, start, count);
            return false;
        }
    }
}
