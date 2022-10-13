using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Generics.BinaryTrees
{
    public static class BinaryTree
    {
        public static BinaryTree<T> Create<T>(params T[] values) where T : struct, IComparable<T>
        {
            var tree = new BinaryTree<T>();
            foreach (T value in values)
            {
                tree.Add(value);
            }
            return tree;
        }
    }
    
    public class BinaryTree<T> : IEnumerable<T> where T : struct, IComparable<T>
    {
        public T Value
        {
            get
            {
                Debug.Assert(_value != null, nameof(_value) + " != null");
                return _value.Value;
            }
        }

        public BinaryTree<T> Left => _left;
        public BinaryTree<T> Right => _right;
        
        private T? _value;
        private BinaryTree<T> _right;
        private BinaryTree<T> _left;

        public BinaryTree(T value)
        {
            _value = value;
        }

        public BinaryTree()
        {
            
        }

        public void Add(T value)
        {
            if (_value == null)
            {
                _value = value;
                return;
            }
            
            if (value.CompareTo(Value) <= 0)
            {
                AddTreeValue(ref _left, value);
            }
            else
            {
                AddTreeValue(ref _right, value);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (Left != null)
            {
                foreach(T leftValue in Left)
                {
                    yield return leftValue;
                }
            }

            if (_value == null)
            {
                yield break;
            }

            yield return Value;

            if (Right == null)
            {
                yield break;
            }
            foreach (T rightValue in Right)
            {
                yield return rightValue;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static void AddTreeValue(ref BinaryTree<T> tree, T value)
        {
            if (tree == null)
            {
                tree = new BinaryTree<T>(value);
            }
            else
            {
                tree.Add(value);
            }
        }
    }
}
