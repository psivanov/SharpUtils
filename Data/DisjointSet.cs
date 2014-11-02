using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Data
{
    public class DisjointSet
    {
        class Node
        {
            public int Label;
            public int Rank;
            public int Size;
        }

        public DisjointSet(int n)
        {
            nodes = new Node[n];
        }

        public void MakeSet(int x)
        {
            Node node = nodes[x];
            if (node == null) node = nodes[x] = new Node();

            node.Label = x;
            node.Rank = 0;
            node.Size = 1;

            m_setCount++;
        }

        public int Find(int x)
        {
            Node node = nodes[x];
            if (node == null)
            {
                MakeSet(x);
                node = nodes[x];
            }

            if (x != node.Label)
            {
                node.Label = Find(node.Label);
            }
            return node.Label;
        }

        public void Union(int x, int y)
        {
            Node xNode = nodes[Find(x)];
            Node yNode = nodes[Find(y)];

            if (xNode != yNode)
            {
                if (xNode.Rank < yNode.Rank)
                {
                    xNode.Label = yNode.Label;
                    yNode.Size += xNode.Size;
                }
                else
                {
                    yNode.Label = xNode.Label;
                    xNode.Size += yNode.Size;
                }

                if (xNode.Rank == yNode.Rank)
                {
                    xNode.Rank++;
                }

                m_setCount--;
            }
        }

        public int Size(int x)
        {
            return nodes[Find(x)].Size;
        }

        public int SetCount()
        {
            return m_setCount;
        }

        private Node[] nodes;
        private int m_setCount;
    }
}
