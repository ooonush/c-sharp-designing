using System;
using System.Collections.Generic;
using System.Text;

namespace Delegates.Observers
{
	public class StackOperationsLogger
	{
		private readonly Logger _logger = new Logger();
		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
			stack.OnStackEvent += _logger.HandleEvent;
		}

		public string GetLog()
		{
			return _logger.Log.ToString();
		}
	}
	
	public class Logger
	{
		public readonly StringBuilder Log = new StringBuilder();
		
		public void HandleEvent(object eventData)
		{
			Log.Append(eventData);
		}
	}

	public class ObservableStack<T>
	{
		private readonly Stack<T> _stack = new Stack<T>();
		public event Action<StackEventData<T>> OnStackEvent;

		public void Push(T item)
		{
			_stack.Push(item);
			OnStackEvent?.Invoke(new StackEventData<T> { IsPushed = true, Value = item });
		}

		public T Pop()
		{
			T result = _stack.Pop();
			OnStackEvent?.Invoke(new StackEventData<T> { IsPushed = false, Value = result });
			return result;
		}
	}
}
