using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Data
{
    public class Trie<T>
    {
        public T Value;
        public bool IsWord = false;
        public Trie<T> Parent = null;
        public Dictionary<T, Trie<T>> Children = new Dictionary<T, Trie<T>>();

        public void AddWord(IEnumerable<T> word)
        {
            Trie<T> current = this;
            foreach (T c in word)
            {
                if (!current.Children.ContainsKey(c))
                {
                    current.Children[c] = new Trie<T>() { Value = c, Parent = current };
                }
                current = current.Children[c];
            }
            if (current != this)
                current.IsWord = true;
        }

        public Trie<T> GetPrefixNode(IEnumerable<T> prefix)
        {
            Trie<T> current = this;
            foreach (T c in prefix)
            {
                if(!current.Children.TryGetValue(c, out current))
                    return null;
            }
            return current;
        }

        public bool Contains(IEnumerable<T> word)
        {
            Trie<T> x = GetPrefixNode(word);
            return x != null && x.IsWord;
        }

        public IEnumerable<T> GetWord()
        {
            IEnumerable<T> ret = Enumerable.Empty<T>();
            if (Parent != null)
            {
                ret = ret.Concat(Parent.GetWord());
                ret = ret.Concat(new T[] { Value });
            }
            return ret;
        }

        public IEnumerable<Trie<T>> GetWordNodes()
        {
            IEnumerable<Trie<T>> ret = Enumerable.Empty<Trie<T>>();
            if (IsWord)
                ret = ret.Concat(new Trie<T>[] { this });
            foreach (Trie<T> t in Children.Values)
                ret = ret.Concat(t.GetWordNodes());
            return ret;
        }
    }
}
