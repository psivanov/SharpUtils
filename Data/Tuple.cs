using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class Tuple<T, K>
    {
        public T Item1;
        public K Item2;

        public Tuple() {}

        public Tuple(T t, K k)
        {
            Item1 = t;
            Item2 = k;
        }

        public override bool Equals(object obj)
        {
            Tuple<T, K> o = (Tuple<T, K>)obj;
            return o.Item1.Equals(Item1) && o.Item2.Equals(Item2);
        }

        public override int GetHashCode()
        {
            return Item1.GetHashCode() ^ Item2.GetHashCode();
        }

        public override string ToString()
        {
            return "(" + Item1 + " " + Item2 + ")";
        }
    }

    public class TupleC<T, K> : Tuple<T, K>, IComparable<TupleC<T, K>>
        where T : IComparable<T>
        where K : IComparable<K>
    {
        #region IComparable<Tuple<T,K>> Members

        public int CompareTo(TupleC<T, K> other)
        {
            int ret = Item1.CompareTo(other.Item1);
            if (ret == 0)
                ret = Item2.CompareTo(other.Item2);
            return ret;
        }

        #endregion
    }

}
