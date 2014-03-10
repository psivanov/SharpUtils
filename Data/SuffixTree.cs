using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Data
{
    //http://stackoverflow.com/questions/9452701/ukkonens-suffix-tree-algorithm-in-plain-english
    //http://www.cise.ufl.edu/~sahni/dsaaj/enrich/c16/suffix.htm
    public class SuffixTree<T>
    {
        public SuffixTree(int capacity)
        {
            text = new List<T>(capacity);
            nodes = new List<Node>(2 * capacity + 2);
            root = activeNode = NewNode(-1, -1);
        }

        public SuffixTree(IEnumerable<T> word)
            : this(word.Count())
        {
            foreach (T ch in word) AddChar(ch);
        }

        public int Root { get { return root; } }
        public int NodeCount { get { return nodes.Count; } }
        public int NodeStart(int node) { return nodes[node].Start; }
        public int NodeEnd(int node) { return Math.Min(nodes[node].End, text.Count); }
        public int NodeSize(int node) { return NodeEnd(node) - NodeStart(node); }
        public IEnumerable<T> NodeText(int node)
        {
            int start = NodeStart(node);
            int end = NodeEnd(node);
            return text.Skip(start).Take(end - start);
        }

        public int Child(int node, T ch)
        {
            int ret;
            if (!nodes[node].Children.TryGetValue(ch, out ret))
            {
                ret = -1;
            }
            return ret;
        }
        public int ChildCount(int node)
        {
            return nodes[node].Children.Count;
        }
        public IEnumerable<int> Children(int node) { return nodes[node].Children.Values; }

        public bool Contains(IList<T> word)
        {
            int n = word.Count;
            int pos = 0;
            int node = Child(Root, word[0]);

            while (node >= 0 && pos < n)
            {
                for (int i = NodeStart(node); i < NodeEnd(node); i++)
                {
                    if (!word[pos++].Equals(text[i])) return false;
                    if (pos == n) return true;
                }
                node = Child(node, word[pos]);
            }

            return false;
        }

        public void AddChar(T ch)
        {
            int position = text.Count;
            text.Add(ch);
            needSuffixLink = -1;
            remainder++;
            while (remainder > 0)
            {
                if (activeLength == 0) activeEdge = position;
                if (!nodes[activeNode].Children.ContainsKey(ActiveEdge()))
                {
                    int leaf = NewNode(position, INF);
                    nodes[activeNode].Children[ActiveEdge()] = leaf;
                    AddSuffixLink(activeNode); //rule 2
                }
                else
                {
                    int next = nodes[activeNode].Children[ActiveEdge()];
                    if (WalkDown(next)) continue;   //observation 2
                    if (ch.Equals(text[nodes[next].Start + activeLength]))
                    {
                        //observation 1
                        activeLength++;
                        AddSuffixLink(activeNode); // observation 3
                        break;
                    }
                    int split = NewNode(nodes[next].Start, nodes[next].Start + activeLength);
                    nodes[activeNode].Children[ActiveEdge()] = split;
                    int leaf = NewNode(position, INF);
                    nodes[split].Children[ch] = leaf;
                    nodes[next].Start += activeLength;
                    nodes[split].Children[text[nodes[next].Start]] = next;
                    AddSuffixLink(split); //rule 2
                }
                remainder--;

                if (activeNode == root && activeLength > 0)
                {
                    //rule 1
                    activeLength--;
                    activeEdge = position - remainder + 1;
                }
                else
                    activeNode = nodes[activeNode].Link > 0 ? nodes[activeNode].Link : root; //rule 3
            }
        }

        #region Private Area

        private void AddSuffixLink(int node)
        {
            if (needSuffixLink > 0)
                nodes[needSuffixLink].Link = node;
            needSuffixLink = node;
        }

        private T ActiveEdge()
        {
            return text[activeEdge];
        }

        private bool WalkDown(int next)
        {
            if (activeLength >= NodeSize(next))
            {
                activeEdge += NodeSize(next);
                activeLength -= NodeSize(next);
                activeNode = next;
                return true;
            }
            return false;
        }

        private int NewNode(int start, int end)
        {
            nodes.Add(new Node(start, end));
            return nodes.Count - 1;
        }

        private class Node
        {
            public int Start, End, Link;
            public Dictionary<T, int> Children = new Dictionary<T, int>();

            public Node(int start, int end)
            {
                this.Start = start;
                this.End = end;
            }
        }

        private int INF = int.MaxValue;
        private List<Node> nodes;
        private List<T> text;
        private int root, needSuffixLink, remainder, activeNode, activeLength, activeEdge;

        #endregion

        #region Visualization

        //graphviz format (dot -Tpng -O st.dot)
        public void PrintTree(System.IO.TextWriter sr)
        {
            sr.WriteLine("digraph {");
            sr.WriteLine("\trankdir = LR;");
            sr.WriteLine("\tedge [arrowsize=0.4,fontsize=10]");
            sr.WriteLine("\tnode1 [label=\"\",style=filled,fillcolor=lightgrey,shape=circle,width=.1,height=.1];");
            sr.WriteLine("//------leaves------");
            PrintLeaves(sr, root);
            sr.WriteLine("//------internal nodes------");
            PrintInternalNodes(sr, root);
            sr.WriteLine("//------edges------");
            PrintEdges(sr, root);
            sr.WriteLine("//------suffix links------");
            PrintSLinks(sr, root);
            sr.WriteLine("}");
        }

        private string EdgeString(int node)
        {
            return string.Join("", NodeText(node));
        }

        private void PrintLeaves(System.IO.TextWriter sr, int x)
        {
            if (ChildCount(x) == 0)
                sr.WriteLine("\tnode" + x + " [label=\"\",shape=point]");
            else
            {
                foreach (int child in Children(x))
                    PrintLeaves(sr, child);
            }
        }

        void PrintInternalNodes(System.IO.TextWriter sr, int x)
        {
            if (x != Root && ChildCount(x) > 0)
                sr.WriteLine("\tnode" + x + " [label=\"\",style=filled,fillcolor=lightgrey,shape=circle,width=.07,height=.07]");

            foreach (int child in Children(x))
                PrintInternalNodes(sr, child);
        }

        void PrintEdges(System.IO.TextWriter sr, int x)
        {
            foreach (int child in Children(x))
            {
                sr.WriteLine("\tnode" + x + " -> node" + child + " [label=\"" + EdgeString(child) + "\",weight=3]");
                PrintEdges(sr, child);
            }
        }

        void PrintSLinks(System.IO.TextWriter sr, int x)
        {
            if (nodes[x].Link > 0)
                sr.WriteLine("\tnode" + x + " -> node" + nodes[x].Link + " [label=\"\",weight=1,style=dotted]");
            foreach (int child in Children(x))
                PrintSLinks(sr, child);
        }
        #endregion
    }
}