using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace RPGCore.Traits
{
	public class TraitCollection<TStat, TState>
		where TStat : IFixedElement, new()
		where TState : IFixedElement, new()
	{
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private TStat[] StatsCache;
		
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private TState[] StatesCache;

		public IEnumerable<TStat> Stats
		{
			get
			{
				if (StatsCache == null)
				{
					StatsCache = GetAllMembers<TStat> ();
				}
				return StatsCache;
			}
		}

		public IEnumerable<TState> States
		{
			get
			{
				if (StatesCache == null)
				{
					StatesCache = GetAllMembers<TState> ();
				}
				return StatesCache;
			}
		}

		private T[] GetAllMembers<T> ()
			where T : IFixedElement, new()
		{
			var targetType = GetType ();

			var foundMembers = new List<T> ();

			foreach (var field in targetType.GetFields (BindingFlags.Public | BindingFlags.Instance))
			{
				if (field.FieldType == typeof (T))
				{
					object fieldValueObject = field.GetValue (this);
					T fieldValue;
					if (fieldValueObject == null)
					{
						fieldValue = new T ();
						field.SetValue (this, fieldValue);
					}
					else
					{
						fieldValue = (T)fieldValueObject;
					}
					
					fieldValue.Name = field.Name;
					fieldValue.Identifier = field.Name;

					foundMembers.Add (fieldValue);
				}
			}

			return foundMembers.ToArray ();
		}
	}
}
