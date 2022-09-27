using System;

namespace Incapsulation.Weights
{
	public class Indexer
	{
		private readonly double[] _array;
		private readonly int _start;
		public readonly int Length;

		public Indexer(double[] array, int start, int length)
		{
			CheckValidate(array, start, length);
			_array = array;
			_start = start;
			Length = length;
		}
		
		public double this[int index]
		{
			get => _array[_start + ValidateIndex(index)];
			set => _array[_start + ValidateIndex(index)] = value;
		}
		
		private int ValidateIndex(int index)
		{
			if (index < 0 || index >= Length)
			{
				throw new IndexOutOfRangeException();
			}

			return index;
		}
		
		private void CheckValidate(double[] array, int start, int length)
		{
			if (start < 0 || start > array.Length)
			{
				throw new ArgumentException();
			}
			
			if (length < 0 || length > array.Length - start)
			{
				throw new ArgumentException();
			}
		}
	}
}
