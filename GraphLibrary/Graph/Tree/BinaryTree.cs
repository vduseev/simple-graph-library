using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Tree
{
    public class BinaryTree<T>
    {
        private BinaryTreeNode<T> root;
        private int count;

        public BinaryTree()
        {
            root = null;
            count = 0;
        }

        public virtual void Clear()
        {
            root = null;
            count = 0;
        }

        public BinaryTreeNode<T> Root
        {
            get
            {
                return root;
            }
            set
            {
                root = value;
            }
        }
        public int Count
        {
            get
            {
                return count;
            }
        }

        public void PreorderTraversal(BinaryTreeNode<T> current)
        {
            if (current != null)
            {
                // Output the value of the current node
                Console.WriteLine(current.Value);

                // Recursively print the left and right children
                PreorderTraversal(current.Left);
                PreorderTraversal(current.Right);
            }
        }

        public void InorderTraversal(BinaryTreeNode<T> current)
        {
            if (current != null)
            {
                // Visit the left child...
                InorderTraversal(current.Left);

                // Output the value of the current node
                Console.WriteLine(current.Value);

                // Visit the right child...
                InorderTraversal(current.Right);
            }
        }

        public bool Contains(T data)
        {
            // search the tree for a node that contains data
            BinaryTreeNode<T> current = root;
            int result;
            while (current != null)
            {
                result = Comparer<T>.Default.Compare(current.Value, data);
                if (result == 0)
                    // we found data
                    return true;
                else if (result > 0)
                    // current.Value > data, search current's left subtree
                    current = current.Left;
                else if (result < 0)
                    // current.Value < data, search current's right subtree
                    current = current.Right;
            }

            return false;       // didn't find data
        }

        public virtual void Add(T data)
        {
            // create a new Node instance
            BinaryTreeNode<T> n = new BinaryTreeNode<T>(data);
            int result;

            // now, insert n into the tree
            // trace down the tree until we hit a NULL
            BinaryTreeNode<T> current = root, parent = null;
            while (current != null)
            {
                result = Comparer<T>.Default.Compare(current.Value, data);
                if (result == 0)
                    // they are equal - attempting to enter a duplicate - do nothing
                    return;
                else if (result > 0)
                {
                    // current.Value > data, must add n to current's left subtree
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // current.Value < data, must add n to current's right subtree
                    parent = current;
                    current = current.Right;
                }
            }

            // We're ready to add the node!
            count++;
            if (parent == null)
                // the tree was empty, make n the root
                root = n;
            else
            {
                result = Comparer<T>.Default.Compare(parent.Value, data);
                if (result > 0)
                    // parent.Value > data, therefore n must be added to the left subtree
                    parent.Left = n;
                else
                    // parent.Value < data, therefore n must be added to the right subtree
                    parent.Right = n;
            }
        }

        public bool Remove(T data)
        {
            Comparer<T> defComp = Comparer<T>.Default;
            // first make sure there exist some items in this tree
            if (root == null)
                return false;       // no items to remove

            // Now, try to find data in the tree
            BinaryTreeNode<T> current = root, parent = null;
            int result = defComp.Compare(current.Value, data);
            while (result != 0)
            {
                if (result > 0)
                {
                    // current.Value > data, if data exists it's in the left subtree
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // current.Value < data, if data exists it's in the right subtree
                    parent = current;
                    current = current.Right;
                }

                // If current == null, then we didn't find the item to remove
                if (current == null)
                    return false;
                else
                    result = defComp.Compare(current.Value, data);
            }

            // At this point, we've found the node to remove
            count--;

            // We now need to "rethread" the tree
            // CASE 1: If current has no right child, then current's left child becomes
            //         the node pointed to by the parent
            if (current.Right == null)
            {
                if (parent == null)
                    root = current.Left;
                else
                {
                    result = defComp.Compare(parent.Value, current.Value);
                    if (result > 0)
                        // parent.Value > current.Value, so make current's left child a left child of parent
                        parent.Left = current.Left;
                    else if (result < 0)
                        // parent.Value < current.Value, so make current's left child a right child of parent
                        parent.Right = current.Left;
                }                
            }
            // CASE 2: If current's right child has no left child, then current's right child
            //         replaces current in the tree
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                if (parent == null)
                    root = current.Right;
                else
                {
                    result = defComp.Compare(parent.Value, current.Value);
                    if (result > 0)
                        // parent.Value > current.Value, so make current's right child a left child of parent
                        parent.Left = current.Right;
                    else if (result < 0)
                        // parent.Value < current.Value, so make current's right child a right child of parent
                        parent.Right = current.Right;
                }
            }
            // CASE 3: If current's right child has a left child, replace current with current's
            //          right child's left-most descendent
            else
            {
                // We first need to find the right node's left-most child
                BinaryTreeNode<T> leftmost = current.Right.Left, lmParent = current.Right;
                while (leftmost.Left != null)
                {
                    lmParent = leftmost;
                    leftmost = leftmost.Left;
                }

                // the parent's left subtree becomes the leftmost's right subtree
                lmParent.Left = leftmost.Right;

                // assign leftmost's left and right to current's left and right children
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;

                if (parent == null)
                    root = leftmost;
                else
                {
                    result = defComp.Compare(parent.Value, current.Value);
                    if (result > 0)
                        // parent.Value > current.Value, so make leftmost a left child of parent
                        parent.Left = leftmost;
                    else if (result < 0)
                        // parent.Value < current.Value, so make leftmost a right child of parent
                        parent.Right = leftmost;
                }
            }

            return true;
        }        
    }
}
