using System;
using System.Collections;
using System.Collections.Generic;

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
        public T? Value { get; private set; }
        public BinaryTree<T> Left { get; private set; }
        public BinaryTree<T> Right { get; private set; }

        public void Add(T value)
        {
            if (Value == null)
            {
                Value = value;
                return;
            }
            
            if (value.CompareTo(Value.Value) <= 0)
            {
                AddToLeft(value);
            }
            else
            {
                AddToRight(value);
            }
        }

        private void AddToLeft(T value)
        {
            if (Left == null)
            {
                Left = new BinaryTree<T>();
            }
            Left.Add(value);
        }
        
        private void AddToRight(T value)
        {
            if (Right == null)
            {
                Right = new BinaryTree<T>();
            }
            Right.Add(value);
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

            if (Value == null)
            {
                yield break;
            }

            yield return Value.Value;

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
    }
}
