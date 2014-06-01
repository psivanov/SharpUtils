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

        public bool RemoveWord(IEnumerable<T> word)
        {
            var node = GetPrefixNode(word);
            if (node == null) return false;

            bool result = node.IsWord;
            node.IsWord = false;

            //remove if needed
            if (node.Children.Count == 0)
            {
                T value = node.Value;
                for (node = node.Parent; node.Parent != null && !node.IsWord && node.Children.Count == 1; node = node.Parent)
                {
                    value = node.Value;
                }
                node.Children.Remove(value);
            }

            return result;
        }

        public Trie<T> GetPrefixNode(IEnumerable<T> prefix)
        {
            Trie<T> current = this;
            foreach (T c in prefix)
            {
                if (!current.Children.TryGetValue(c, out current))
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
            if (Parent != null)
            {
                foreach (T ch in Parent.GetWord())
                    yield return ch;

                yield return Value;
            }
        }

        public IEnumerable<Trie<T>> GetWordNodes()
        {
            if (IsWord)
                yield return this;
            foreach (Trie<T> child in Children.Values)
                foreach (Trie<T> word in child.GetWordNodes())
                    yield return word;
        }
    }
}
