using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static partial class Alg
    {
        public class InversionCount<T> where T : IComparable<T>
        {
            private T[] numbers;
            private long swaps;

            public InversionCount(IEnumerable<T> numbers)
            {
                this.numbers = numbers.ToArray();
                this.swaps = 0;
            }

            public void MergeSortCount()
            {
                MergeSortCount(0, numbers.Length - 1);
            }

            public void MergeSortCount(int start, int end)
            {
                if (start < end)
                {
                    int middle = (start + end) / 2;
                    MergeSortCount(start, middle);
                    MergeSortCount(middle + 1, end);
                    MergeCount(start, middle, end);
                }
            }

            protected void MergeCount(int start, int middle, int end)
            {
                int firstLength = middle - start + 1;
                T[] first = new T[firstLength];
                Array.Copy(numbers, start, first, 0, firstLength);

                int secondLength = end - middle;
                T[] second = new T[secondLength];
                Array.Copy(numbers, middle + 1, second, 0, secondLength);

                int firstIndex = 0;
                int secondIndex = 0;
                int index = start;

                while (firstIndex < firstLength && secondIndex < secondLength)
                {
                    if (first[firstIndex].CompareTo(second[secondIndex]) <= 0)
                    {
                        numbers[index++] = first[firstIndex++];
                    }
                    else
                    {
                        numbers[index++] = second[secondIndex++];
                        swaps += firstLength - firstIndex;
                    }
                }

                while (firstIndex < firstLength)
                {
                    numbers[index++] = first[firstIndex++];
                }

                while (secondIndex < secondLength)
                {
                    numbers[index++] = second[secondIndex++];
                }
            }

            public long GetSwaps()
            {
                return swaps;
            }
        }
    }
}