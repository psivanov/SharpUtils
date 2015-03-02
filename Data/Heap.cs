using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Data
{
    public class Heap<T> where T : IComparable<T>
    {
        public Heap() : this(false) { }
        public Heap(ICollection<T> collection) : this(collection, false) { }
        public Heap(ICollection<T> collection, bool minHeap)
            : this(collection.Count, minHeap)
        {
            foreach (T t in collection)
                Push(t);
        }
        public Heap(bool minHeap) : this(16, minHeap) { }
        public Heap(int capacity) : this(capacity, false) { }
        public Heap(int capacity, bool minHeap)
        {
            int cap = 2;
            while (cap < capacity) cap <<= 1;

            heap = new T[cap + 1];
            size = 0;
            this.minHeap = minHeap;
        }

        public int Count { get { return size; } }

        public bool IsEmpty() { return size < 1; }

        public T Peek()
        {
            if (size < 1)
                throw new ApplicationException("The heap is empty!");
            return heap[1];
        }

        public virtual T Pop()
        {
            if (size < 1)
                throw new ApplicationException("The heap is empty!");
            T max = Peek();
            heap[1] = heap[size--];
            inverse[heap[1]] = 1;
            Heapify(1);
            inverse.Remove(max);
            return max;
        }

        public virtual void Push(T value)
        {
            if (++size > heap.Length - 1)
                Array.Resize<T>(ref heap, heap.Length << 1);
            int i = size;
            while (i > 1 && ((!minHeap && heap[P(i)].CompareTo(value) < 0) || (minHeap && heap[P(i)].CompareTo(value) > 0)))
            {
                heap[i] = heap[P(i)];
                inverse[heap[i]] = i;
                i = P(i);
            }
            heap[i] = value;
            inverse[value] = i;
        }

        public virtual T PopPush(T value)
        {
            if (size < 1)
                throw new ApplicationException("The heap is empty!");
            T max = Peek();
            heap[1] = value;
            inverse[value] = 1;
            Heapify(1);
            return max;
        }

        public void Update(T old, T value)
        {
            int i = -1;
            if (old.CompareTo(value) != 0 && inverse.TryGetValue(old, out i))
            {
                inverse.Remove(old);

                heap[i] = value;
                inverse[value] = i;

                if ((!minHeap && old.CompareTo(value) > 0) || (minHeap && old.CompareTo(value) < 0))
                {
                    Heapify(i);
                }
                else
                {
                    while (i > 1 && ((!minHeap && heap[i].CompareTo(heap[P(i)]) > 0) || (minHeap && heap[i].CompareTo(heap[P(i)]) < 0)))
                    {
                        Swap(i, P(i));
                        i = P(i);
                    }
                }
            }
        }

        protected int P(int i)
        {
            return i / 2;
        }
        protected int L(int i)
        {
            return 2 * i;
        }
        protected int R(int i)
        {
            return 2 * i + 1;
        }
        protected void Heapify(int i)
        {
            int max = i;
            int l = L(i);
            int r = R(i);
            if (l <= size && ((!minHeap && heap[l].CompareTo(heap[i]) > 0) || (minHeap && heap[l].CompareTo(heap[i]) < 0)))
                max = l;
            if (r <= size && ((!minHeap && heap[r].CompareTo(heap[max]) > 0) || (minHeap && heap[r].CompareTo(heap[max]) < 0)))
                max = r;
            if (max != i)
            {
                Swap(i, max);
                Heapify(max);
            }
        }

        protected void Swap(int i, int j)
        {
            T ti = heap[i];
            T tj = heap[j];
            heap[j] = ti;
            heap[i] = tj;
            inverse[ti] = j;
            inverse[tj] = i;
        }

        protected T[] heap;
        protected Dictionary<T, int> inverse = new Dictionary<T, int>();
        protected int size;
        protected bool minHeap;
    }

    public class HeapNode<D, P> : IComparable<HeapNode<D, P>> where P : IComparable<P>
    {
        public HeapNode(D data, P priority)
        {
            this.Data = data;
            this.Priority = priority;
        }

        public D Data;
        public P Priority;

        public int CompareTo(HeapNode<D, P> other)
        {
            return Priority.CompareTo(other.Priority);
        }

        public override bool Equals(object obj)
        {
            return obj is HeapNode<D, P> && object.Equals(Data, ((HeapNode<D, P>)obj).Data);
        }

        public override int GetHashCode()
        {
            return object.Equals(Data, default(D)) ? 0 : Data.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Data, Priority);
        }
    }

    public class Heap<D, P> : Heap<HeapNode<D, P>> where P : IComparable<P>
    {
        public Heap() : base() { }
        public Heap(bool minHeap) : base(minHeap) { }
        public Heap(int capacity) : base(capacity) { }
        public Heap(int capacity, bool minHeap) : base(capacity, minHeap) { }

        public override HeapNode<D, P> Pop()
        {
            var popped = base.Pop();
            lookup.Remove(popped.Data);
            return popped;
        }
        public override void Push(HeapNode<D, P> value)
        {
            lookup[value.Data] = value;
            base.Push(value);
        }

        public override HeapNode<D, P> PopPush(HeapNode<D, P> value)
        {
            lookup[value.Data] = value;
            var popped = base.PopPush(value);
            lookup.Remove(popped.Data);
            return popped;
        }

        public D PeekData() { return Peek().Data; }
        public P PeekPriority() { return Peek().Priority; }
        public void Push(D data, P priority) { Push(new HeapNode<D, P>(data, priority)); }
        public HeapNode<D, P> PopPush(D data, P priority) { return PopPush(new HeapNode<D, P>(data, priority)); }

        public void Update(D data, P priority)
        {
            if (lookup.ContainsKey(data))
            {
                var node = lookup[data];
                P oldPriority = node.Priority;

                int i = inverse[node];
                node.Priority = priority;

                if ((!minHeap && oldPriority.CompareTo(priority) > 0) || (minHeap && oldPriority.CompareTo(priority) < 0))
                {
                    Heapify(i);
                }
                else
                {
                    while (i > 1 && ((!minHeap && heap[i].CompareTo(heap[P(i)]) > 0) || (minHeap && heap[i].CompareTo(heap[P(i)]) < 0)))
                    {
                        Swap(i, P(i));
                        i = P(i);
                    }
                }
            }
        }

        public bool Contains(D data)
        {
            return lookup.ContainsKey(data);
        }

        public bool TryGetPriority(D data, out P priority)
        {
            HeapNode<D, P> node;
            bool result = lookup.TryGetValue(data, out node);
            priority = node.Priority;
            return result;
        }

        private Dictionary<D, HeapNode<D, P>> lookup = new Dictionary<D, HeapNode<D, P>>();
    }
}
