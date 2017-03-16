using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Data
{
    //Red-Black Tree with generic augmentation mechanism
    public class RBTree<K, V, A> : IEnumerable<RBTree<K, V, A>.RBNode> where K : IComparable<K>
    {
        public class RBNode
        {
            public K Key;
            public V Value;
            public A AValue;
            public RBNode Parent;
            public RBNode Left;
            public RBNode Right;

            internal bool Black;

            public RBTree<K, V, A> Tree;

            public RBNode(K k, V v, RBTree<K, V, A> tree)
            {
                Tree = tree;
                Key = k;
                Value = v;
                Parent = Left = Right = Tree.Null;
            }

            public bool IsRoot() { return Parent == Tree.Null; }
            public bool IsLeft() { return !IsRoot() && Parent.Left == this; }
            public bool IsRight() { return !IsRoot() && Parent.Right == this; }
            public RBNode Unbox() { return this != Tree.Null ? this : null; }
        }

        public interface IAugmentor
        {
            //augmented value for the null node
            A GetNullAValue();
            //this function needs to only depend on node, node.Left and node.Right
            void UpdateAValue(RBNode node);
        }

        public readonly RBNode Null;
        public RBNode Root;
        private IAugmentor Augmentor;

        public RBTree(IAugmentor augmentor = null)
        {
            Augmentor = augmentor;
            Null = new RBNode(default(K), default(V), this);
            Null.Black = true;

            if (Augmentor != null)
                Null.AValue = augmentor.GetNullAValue();

            Root = Null;
        }

        #region General Collection methods

        //IEnumerable
        public IEnumerator<RBNode> GetEnumerator()
        {
            return Enumerate(Root).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Enumerate(Root).GetEnumerator();
        }
        private IEnumerable<RBNode> Enumerate(RBNode node)
        {
            if (node != Null)
            {
                return Enumerate(node.Left).Concat(Enumerable.Repeat(node, 1)).Concat(Enumerate(node.Right));
            }
            return Enumerable.Empty<RBNode>();
        }

        public bool TryGetValue(K key, out V value)
        {
            RBNode node = Find(key);
            if (node != null)
            {
                value = node.Value;
                return true;
            }
            value = default(V);
            return false;
        }
        public V Get(K key)
        {
            V value;
            TryGetValue(key, out value);
            return value;
        }

        public RBNode Add(K key, V value)
        {
            RBNode node = new RBNode(key, value, this);
            Add(node);
            return node;
        }

        public bool Remove(K key)
        {
            RBNode node = Find(key);
            if (node != null)
            {
                Remove(node);
                return true;
            }
            return false;
        }

        public bool IsEmpty() { return Root != Null; }

        #endregion


        #region Red-Black Tree methods

        public void Add(RBNode node)
        {
            AddBST(node);
            node.Black = false;
            while(node != Root && !node.Parent.Black)
            {
                if (node.Parent.IsLeft())
                {
                    RBNode uncle = node.Parent.Parent.Right;
                    if (!uncle.Black)
                    {
                        node.Parent.Black = true;
                        uncle.Black = true;
                        node = node.Parent.Parent;
                        node.Black = false;
                    }
                    else
                    {
                        if(node.IsRight())
                        {
                            node = node.Parent;
                            RotateLeft(node);
                        }
                        //node is left now
                        node.Parent.Black = true;
                        node.Parent.Parent.Black = false;
                        RotateRight(node.Parent.Parent);
                    }
                }
                else //node parent is right
                {
                    RBNode uncle = node.Parent.Parent.Left;
                    if (!uncle.Black)
                    {
                        node.Parent.Black = true;
                        uncle.Black = true;
                        node = node.Parent.Parent;
                        node.Black = false;
                    }
                    else
                    {
                        if (node.IsLeft())
                        {
                            node = node.Parent;
                            RotateRight(node);
                        }
                        //node is right now
                        node.Parent.Black = true;
                        node.Parent.Parent.Black = false;
                        RotateLeft(node.Parent.Parent);
                    }
                }
            }
            Root.Black = true;
        }

        public void Remove(RBNode node)
        {
            RBNode deleteNode = node;
            if (node.Left != Null && node.Right != Null)
                deleteNode = Next(node);

            RBNode child = deleteNode.Left != Null ? deleteNode.Left : deleteNode.Right;
            child.Parent = deleteNode.Parent;

            if (deleteNode.IsRoot())
                Root = child;
            else if (deleteNode.IsLeft())
                deleteNode.Parent.Left = child;
            else
                deleteNode.Parent.Right = child;

            //augmented value
            PropagateAValue(child);

            bool needFixup = deleteNode.Black;

            if (deleteNode != node)
            {
                //swap
                //parent
                if (node.IsRoot())
                    Root = deleteNode;
                else if (node.IsLeft())
                    node.Parent.Left = deleteNode;
                else
                    node.Parent.Right = deleteNode;

                deleteNode.Parent = node.Parent;

                //left
                if (node.Left != Null)
                    node.Left.Parent = deleteNode;

                deleteNode.Left = node.Left;

                //right
                if (node.Right != Null)
                    node.Right.Parent = deleteNode;

                deleteNode.Right = node.Right;

                //augmented value
                PropagateAValue(deleteNode);

                //color
                deleteNode.Black = node.Black;
            }

            if (needFixup)
                RemoveFixup(child);
        }

        private void RotateLeft(RBNode node)
        {
            RBNode child = node.Right;

            //child's left becomes node's right
            node.Right = child.Left;
            child.Left.Parent = node;

            //node parent link to child
            child.Parent = node.Parent;
            if (node.IsRoot())
                Root = child;
            else if (node.IsLeft())
                node.Parent.Left = child;
            else
                node.Parent.Right = child;

            //node becomes the left on child
            child.Left = node;
            node.Parent = child;

            //augmented value
            PropagateAValue(node);
        }
        private void RotateRight(RBNode node)
        {
            RBNode child = node.Left;

            //child's right becomes node's left
            node.Left = child.Right;
            child.Right.Parent = node;

            //node parent link to child
            child.Parent = node.Parent;
            if (node.IsRoot())
                Root = child;
            else if (node.IsLeft())
                node.Parent.Left = child;
            else
                node.Parent.Right = child;

            //node becomes the right on child
            child.Right = node;
            node.Parent = child;

            //augmented value
            PropagateAValue(node);
        }
        
        private void RemoveFixup(RBNode node)
        {
            while (!node.IsRoot() && node.Black)
            {
                if (node.IsLeft())
                {
                    RBNode sibling = node.Parent.Right;
                    if (!sibling.Black)
                    {
                        sibling.Black = true;
                        node.Parent.Black = false;
                        RotateLeft(node.Parent);
                        sibling = node.Parent.Right;
                    }
                    //sibling is black

                    if(sibling.Left.Black && sibling.Right.Black)
                    {
                        sibling.Black = false;
                        node = node.Parent;
                    }
                    else
                    {
                        if (sibling.Right.Black)
                        {
                            sibling.Left.Black = true;
                            sibling.Black = false;
                            RotateRight(sibling);
                            sibling = node.Parent.Right;
                        }
                        //sibling.Right is red

                        sibling.Black = node.Parent.Black;
                        node.Parent.Black = true;
                        sibling.Right.Black = true;
                        RotateLeft(node.Parent);
                        node = Root; //stop loop
                    }
                }
                else //node is right
                {
                    RBNode sibling = node.Parent.Left;
                    if (!sibling.Black)
                    {
                        sibling.Black = true;
                        node.Parent.Black = false;
                        RotateRight(node.Parent);
                        sibling = node.Parent.Left;
                    }
                    //sibling is black

                    if (sibling.Left.Black && sibling.Right.Black)
                    {
                        sibling.Black = false;
                        node = node.Parent;
                    }
                    else
                    {
                        if (sibling.Left.Black)
                        {
                            sibling.Right.Black = true;
                            sibling.Black = false;
                            RotateLeft(sibling);
                            sibling = node.Parent.Left;
                        }
                        //sibling.Left is red

                        sibling.Black = node.Parent.Black;
                        node.Parent.Black = true;
                        sibling.Left.Black = true;
                        RotateRight(node.Parent);
                        node = Root; //stop loop
                    }
                }
            }
            node.Black = true;
        }

        #endregion


        #region BST methods

        //insert
        private void AddBST(RBNode newNode)
        {
            RBNode parent = Null;
            RBNode node = Root;
            while (node != Null)
            {
                parent = node;
                if (newNode.Key.CompareTo(node.Key) < 0)
                    node = node.Left;
                else
                    node = node.Right;
            }

            newNode.Parent = parent;
            if (parent == Null)
                Root = newNode;
            else if (newNode.Key.CompareTo(parent.Key) < 0)
                parent.Left = newNode;
            else
                parent.Right = newNode;

            //augmented value
            PropagateAValue(newNode);
        }

        //thin wrappers
        public RBNode Find(K key) { return Find(Root, key); }
        public RBNode Min() { return Min(Root); }
        public RBNode Max() { return Max(Root); }

        #endregion


        #region Node Methods

        public RBNode Find(RBNode node, K key)
        {
            while (node != Null)
            {
                int comp = key.CompareTo(node.Key);

                if (comp < 0)
                    node = node.Left;
                else if (comp > 0)
                    node = node.Right;
                else
                    break;
            }

            return node.Unbox();
        }

        public RBNode Min(RBNode node)
        {
            if (node != Null)
                while (node.Left != Null)
                    node = node.Left;

            return node.Unbox();
        }
        public RBNode Max(RBNode node)
        {
            if (node != Null)
                while (node.Right != Null)
                    node = node.Right;

            return node.Unbox();
        }

        public RBNode Next(RBNode node)
        {
            if (node.Right != Null)
                return Min(node.Right);

            RBNode parent = node.Parent;
            while (parent != Null && node == parent.Right)
            {
                node = parent;
                parent = node.Parent;
            }
            return parent;
        }
        public RBNode Prev(RBNode node)
        {
            if (node.Left != Null)
                return Max(node.Left);

            RBNode parent = node.Parent;
            while (parent != Null && node == parent.Left)
            {
                node = parent;
                parent = node.Parent;
            }
            return parent;
        }

        public void PropagateAValue(RBNode node)
        {
            if (Augmentor != null)
            {
                while (node != null)
                {
                    if (node != Null)
                        Augmentor.UpdateAValue(node);
                    if (node == Root)
                        break;
                    node = node.Parent;
                }
            }
        }

        #endregion
    }
}