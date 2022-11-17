using System;
using System.Collections.Generic;
using System.Linq;

namespace Delegates.TreeTraversal
{
    public static class Traversal
    {
        public static IEnumerable<Product> GetProducts(ProductCategory root)
        {
            return GetValues(
                root,
                _ => true,
                category => category.Products,
                category => category.Categories
            );
        }

        public static IEnumerable<Job> GetEndJobs(Job root)
        {
            return GetValues(
                root,
                job => job.Subjobs.Count == 0,
                job => new[] { job },
                job => job.Subjobs);
        }

        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
        {
            return GetValues(
                root,
                tree => tree.Left == null && tree.Right == null,
                tree => new[] { tree.Value },
                TravelBinaryTree
            );

            IEnumerable<BinaryTree<T>> TravelBinaryTree(BinaryTree<T> tree)
            {
                if (tree.Left != null)
                {
                    yield return tree.Left;
                }

                if (tree.Right != null)
                {
                    yield return tree.Right;
                }
            }
        }

        private static IEnumerable<TResult> GetValues<T, TResult>(
            T value,
            Func<T, bool> predicate,
            Func<T, IEnumerable<TResult>> selector,
            Func<T, IEnumerable<T>> traveler)
        {
            if (predicate(value))
            {
                foreach (TResult result in selector(value))
                {
                    yield return result;
                }
            }

            var subResults = traveler(value)
                .SelectMany(subValue => GetValues(subValue, predicate, selector, traveler));
            foreach (TResult subResult in subResults)
            {
                yield return subResult;
            }
        }
    }
}