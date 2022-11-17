using System;
using System.Collections.Generic;
using System.Linq;

namespace Delegates.PairsAnalysis
{
    public static class Analysis
    {
        public static int FindMaxPeriodIndex(params DateTime[] data)
        {
            return data.Pairs().Select(tuple => (tuple.Item2 - tuple.Item1).TotalSeconds).MaxIndex();
        }

        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> items)
        {
            var enumerator = items.GetEnumerator();
            enumerator.MoveNext();
            T prev = enumerator.Current;

            while (enumerator.MoveNext())
            {
                yield return Tuple.Create(prev, enumerator.Current);
                prev = enumerator.Current;
            }
            
            enumerator.Dispose();
        }

        public static int MaxIndex<T>(this IEnumerable<T> items) where T : IComparable<T>
        {
            var enumerator = items.GetEnumerator();
            if (!enumerator.MoveNext()) throw new InvalidOperationException();
            
            T max = enumerator.Current;
            int bestIndex = 0;
            
            for (int i = 1; enumerator.MoveNext(); i++)
            {
                if (enumerator.Current != null && enumerator.Current.CompareTo(max) > 0)
                {
                    max = enumerator.Current;
                    bestIndex = i;
                }
            }
            
            enumerator.Dispose();
            return bestIndex;
        }

        public static double FindAverageRelativeDifference(params double[] data)
        {
            return data.Pairs().Average(tuple => (tuple.Item2 - tuple.Item1) / tuple.Item1);
        }
    }
}