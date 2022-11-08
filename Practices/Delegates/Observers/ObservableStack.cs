using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Observers
{

	public class StackOperationsLogger
	{
		private readonly Observer observer = new Observer();
		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
			stack.Add(observer);
		}

		public string GetLog()
		{
			return observer.Log.ToString();
		}
	}

	public interface IObserver
	{
		void HandleEvent(object eventData);
	}

	public class Observer : IObserver
	{
		public StringBuilder Log = new StringBuilder();

		public void HandleEvent(object eventData)
		{
			Log.Append(eventData);
		}
	}

	public interface IObservable
	{
		void Add(IObserver observer);
		void Remove(IObserver observer);
		void Notify(object eventData);
	}


	public class ObservableStack<T> : IObservable
	{
		List<IObserver> observers = new List<IObserver>();

		public void Add(IObserver observer)
		{
			observers.Add(observer);
		}

		public void Notify(object eventData)
		{
			foreach (var observer in observers)
				observer.HandleEvent(eventData);
		}

		public void Remove(IObserver observer)
		{
			observers.Remove(observer);
		}

		List<T> data = new List<T>();

		public void Push(T obj)
		{
			data.Add(obj);
			Notify(new StackEventData<T> { IsPushed = true, Value = obj });
		}

		public T Pop()
		{
			if (data.Count == 0)
				throw new InvalidOperationException();
			var result = data[data.Count - 1];
			Notify(new StackEventData<T> { IsPushed = false, Value = result });
			return result;

		}
	}


}
