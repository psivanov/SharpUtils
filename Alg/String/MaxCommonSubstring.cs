using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Data;

namespace Utils
{
    public static partial class Alg
    {
        //Finds the longest common substring in a list of words.
        //Maximum number of words: 64
        //If there are more than one substrings, it returns the one tht appears first in the first word.
        public class MaxCommonSubstring<T>
        {
            public MaxCommonSubstring(int textCapacity = 1000)
            {
                suffTree = new SuffixTree<TW>(textCapacity);
                wordStart[0] = 0;
            }

            public void AddString(IEnumerable<T> word)
            {
                if (wordCount >= MAX_WORDS) throw new ApplicationException("Max words reached!");

                int charPos = wordStart[wordCount];
                foreach(T ch in word)
                {
                    charPos++;
                    suffTree.AddChar(new TW(ch));
                }
                suffTree.AddChar(new TW());
                charPos++;

                wordStart[++wordCount] = charPos;
            }

            public int Go(out int startPos)
            {
                int nodeCount = suffTree.NodeCount;
                if (signatureMemo == null) signatureMemo = new Signature[nodeCount];
                startPos = int.MaxValue;
                int length = FindBestNode(suffTree.Root, 0, ref startPos);
                startPos -= length;
                return length;
            }

            private Signature GetSignature(int node)
            {
                int nodeStart = suffTree.NodeStart(node);
                if (signatureMemo[node] == null)
                {
                    signatureMemo[node] = new Signature();
                    if (suffTree.ChildCount(node) > 0)
                    {
                        foreach (int child in suffTree.Children(node))
                        {
                            Signature childSignature = GetSignature(child);
                            signatureMemo[node].Mask |= childSignature.Mask;
                            if ((childSignature.Mask & 1) != 0)
                            {
                                int childMinPos = childSignature.MinPos;
                                if (suffTree.ChildCount(child) > 0)
                                    childMinPos -= suffTree.NodeSize(child);
                                signatureMemo[node].MinPos = Math.Min(signatureMemo[node].MinPos, childMinPos);
                            }
                        }
                    }
                    else
                    {
                        long mask = 1;
                        for (int word = 1; word <= wordCount; word++, mask <<= 1)
                        {
                            if (nodeStart < wordStart[word])
                            {
                                signatureMemo[node].Mask |= mask;
                                if (word == 1)
                                    signatureMemo[node].MinPos = nodeStart;
                                break;
                            }
                        }
                    }
                }

                return signatureMemo[node];
            }

            private int FindBestNode(int startNode, int size, ref int minPos)
            {
                Signature signature = GetSignature(startNode);
                if (signature.Mask == (1 << wordCount) - 1)
                {
                    minPos = signature.MinPos;
                    size += suffTree.NodeSize(startNode);

                    int maxSize = size;
                    int childMinPos = int.MaxValue;
                    foreach (int child in suffTree.Children(startNode))
                    {
                        int ss = FindBestNode(child, size, ref childMinPos);
                        if (maxSize < ss || (maxSize == ss && childMinPos < minPos))
                        {
                            maxSize = ss;
                            minPos = childMinPos;
                        }
                    }

                    return maxSize;
                }

                return size;
            }

            private const int MAX_WORDS = 64;
            private List<TW> text = new List<TW>();
            private int wordCount = 0;
            private int[] wordStart = new int[MAX_WORDS];
            private SuffixTree<TW> suffTree;

            private Signature[] signatureMemo;

            private class TW
            {
                private T value;
                private bool isUnique = true;

                public TW() { }
                public TW(T value) { this.value = value; isUnique = false; }

                public override bool Equals(object obj)
                {
                    TW o = obj as TW;
                    if (!isUnique && o != null && !o.isUnique)
                    {
                        return value.Equals(o.value);
                    }

                    return false;
                }

                public override int GetHashCode()
                {
                    if (!isUnique) return value.GetHashCode();

                    return base.GetHashCode();
                }
            }

            private class Signature
            {
                public long Mask;
                public int MinPos = int.MaxValue;
            }
        }
    }
}
