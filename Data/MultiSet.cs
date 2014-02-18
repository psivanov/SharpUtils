using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utils.Data
{
    public class MultiSet<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        //Constructors
        public MultiSet() : this((IComparer<T>)null) { }
        public MultiSet(IComparer<T> comparer)
        {
            _itemComparer = new ItemComparer(comparer);
            _set = new SortedSet<Item>(_itemComparer);
            _count = 0;
            _version = 0;
        }
        public MultiSet(IEnumerable<T> collection) : this(collection, (IComparer<T>)null) { }
        public MultiSet(IEnumerable<T> collection, IComparer<T> comparer)
            : this(comparer)
        {
            foreach (T item in collection)
            {
                Add(item);
            }
        }
        public MultiSet(MultiSet<T> other)
        {
            _itemComparer = other._itemComparer;
            _set = new SortedSet<Item>(other._set, _itemComparer);
            _count = other._count;
            _version = 0;
        }

        //IEnumerable
        public Enumerator GetEnumerator()
        {
            return new MultiSet<T>.Enumerator(this);
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new MultiSet<T>.Enumerator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MultiSet<T>.Enumerator(this);
        }

        //ICollection
        public void Add(T item)
        {
            _version++;
            _count++;

            Item x = Find(item);

            if (x == null)
            {
                _set.Add(new Item(item));
            }
            else
            {
                x.Value++;
            }
        }

        public void Clear()
        {
            _version++;
            _count = 0;
            _set.Clear();
        }

        public bool Contains(T item)
        {
            return _set.Contains(new Item(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return _count;  }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            Item x = Find(item);

            if (x != null)
            {
                _version++;
                _count--;

                if (x.Value < 2)
                {
                    _set.Remove(x);
                }
                else
                {
                    x.Value--;
                }

                return true;
            }

            return false;
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
                }
                return this._syncRoot;
            }
        }

        //new

        public T Min
        {
            get
            {
                Item item = _set.Min;
                return item != null ? item.Key : default(T);
            }
        }

        public T Max
        {
            get
            {
                Item item = _set.Max;
                return item != null ? item.Key : default(T);
            }
        }

        public int CountSingle(T item)
        {
            var found = Find(item);
            return found != null ? found.Value : 0;
        }

        public bool RemoveAll(T item)
        {
            Item x = Find(item);

            if (x != null)
            {
                _version++;
                _count -= x.Value;
                _set.Remove(x);
 
                return true;
            }

            return false;
        }

        public IEnumerable<T> Reverse()
        {
            foreach (Item item in _set.Reverse())
            {
                for (int i = 0; i < item.Value; i++)
                {
                    yield return item.Key;
                }
            }
        }

        public IEnumerable<T> SubSet(T lower, T upper)
        {
            var lowerItem = new Item(lower);
            var upperItem = new Item(upper);
            if (_itemComparer.Compare(lowerItem, upperItem) <= 0)
            {
                foreach (Item item in _set.GetViewBetween(new Item(lower), new Item(upper)))
                {
                    for (int i = 0; i < item.Value; i++)
                    {
                        yield return item.Key;
                    }
                }
            }
        }

        public int SubSetCount(T lower, T upper)
        {
            int ret = 0;
            var lowerItem = new Item(lower);
            var upperItem = new Item(upper);
            if (_itemComparer.Compare(lowerItem, upperItem) <= 0)
            {
                ret = _set.GetViewBetween(lowerItem, upperItem).Count;
            }

            return ret;
        }

        private Item Find(T key)
        {
            var item = new Item(key);
            var view = _set.GetViewBetween(item, item);
            return view.FirstOrDefault();
        }

        private SortedSet<Item> _set;
        private ItemComparer _itemComparer;
        private int _count = 0;
        private int _version = 0;
        private object _syncRoot;



        private class Item
        {
            public Item(T key)
            {
                Key = key;
                Value = 1;
            }
            public T Key;
            public int Value;
        }

        private class ItemComparer : IComparer<Item>
        {
            public ItemComparer(IComparer<T> comparer)
            {
                _comparer = comparer;
                if (_comparer == null)
                {
                    _comparer = Comparer<T>.Default;
                }
            }

            public int Compare(Item x, Item y)
            {
                return _comparer.Compare(x.Key, y.Key);
            }

            private IComparer<T> _comparer;
        }

        public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            public Enumerator(MultiSet<T> multiSet)
            {
                _multiSet = multiSet;
                _enumerator = _multiSet._set.GetEnumerator();
                _currentCount = 0;
                _version = _multiSet._version;
            }

            public T Current
            {
                get
                {
                    CheckVersion();
                    return _enumerator.Current != null ? _enumerator.Current.Key : default(T);
                }
            }

            public void Dispose()
            {
                _enumerator.Dispose();
            }

            public bool MoveNext()
            {
                CheckVersion();
                Item item = _enumerator.Current;
                if (item != null && _currentCount < item.Value-1)
                {
                    _currentCount++;
                    return true;
                }

                _currentCount = 0;
                return _enumerator.MoveNext();
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Reset()
            {
                CheckVersion();
                ((IEnumerator)_enumerator).Reset();
                _currentCount = 0;
            }

            private void CheckVersion()
            {
                if (_version != _multiSet._version)
                {
                    throw new InvalidOperationException("");
                }
            }

            private MultiSet<T> _multiSet;
            private SortedSet<Item>.Enumerator _enumerator;
            private int _currentCount;
            private int _version;
        }
    }
}