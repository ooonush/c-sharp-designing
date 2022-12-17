using System.Linq;
using System.Reflection;

namespace Ddd.Infrastructure
{
	/// <summary>
	/// Базовый класс для всех Value типов.
	/// </summary>
	public abstract class ValueType<T> where T : ValueType<T>
	{
		private readonly PropertyInfo[] _properties;
		
		protected ValueType()
		{
			var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			_properties = properties.OrderBy(x => x.Name).ToArray();
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj)) return true;
			
			if (!(obj is ValueType<T> other)) return false;
			
			foreach (PropertyInfo property in _properties)
			{
				object value = property.GetValue(this);
				object otherValue = property.GetValue(other);
				
				if (!Equals(value, otherValue)) return false;
			}

			return true;
		}

		public bool Equals(T other)
		{
			return Equals((object)other);
		}

		public override int GetHashCode()
		{
			var hash = 0;

			for (var i = 0; i < _properties.Length; i++)
			{
				PropertyInfo property = _properties[i];
				unchecked
				{
					object propertyValue = property.GetValue(this);
					if (propertyValue != null)
					{
						hash += propertyValue.GetHashCode() * (i + 666);
					}
				}
			}

			return hash;
		}
		
		public override string ToString()
		{
			string name = GetType().Name;
			var propertyValues = _properties.Select(p => p.Name + ": " + p.GetValue(this));
			return name + "(" + string.Join("; ", propertyValues) + ")";
		}
	}
}