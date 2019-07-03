using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public class EventField<T> : IDisposable
	{
		public struct HandlerCollection : IDisposable
		{
			public struct ContextWrapped
			{
				public IEventFieldHandler Result;

				private readonly EventField<T> Field;
				private readonly object Context;

				public ContextWrapped (EventField<T> field, object context)
				{
					Field = field;
					Context = context;
					Result = null;
				}

				public void Clear ()
				{
					Field.Handlers.Clear (Context);
				}

				public static ContextWrapped operator + (ContextWrapped left, IEventFieldHandler right)
				{
					left.Result = right;
					return left;
				}
			}

			private readonly EventField<T> field;
			private List<KeyValuePair<object, IEventFieldHandler>> handlers;

			public HandlerCollection (EventField<T> field)
			{
				this.field = field;
				handlers = null;
			}

			public ContextWrapped this[object context]
			{
				get => new ContextWrapped (field, context);
				set
				{
					if (handlers == null)
					{
						handlers = new List<KeyValuePair<object, IEventFieldHandler>> ();
					}

					handlers.Add (new KeyValuePair<object, IEventFieldHandler> (context, value.Result));
				}
			}

			public void Clear (object context)
			{
				for (int i = handlers.Count - 1; i >= 0; i--)
				{
					if (handlers[i].Key == context)
					{
						handlers.RemoveAt (i);
					}
				}
			}

			public void Clear ()
			{
				handlers.Clear ();
			}

			public void InvokeBeforeChanged ()
			{
				if (handlers == null)
				{
					return;
				}

				for (int i = 0; i < handlers.Count; i++)
				{
					handlers[i].Value.OnBeforeChanged ();
				}
			}

			public void InvokeAfterChanged ()
			{
				if (handlers == null)
				{
					return;
				}

				for (int i = 0; i < handlers.Count; i++)
				{
					handlers[i].Value.OnAfterChanged ();
				}
			}

			public void Dispose ()
			{
				if (handlers == null)
				{
					return;
				}

				for (int i = 0; i < handlers.Count; i++)
				{
					handlers[i].Value.Dispose ();
				}
			}
		}



		public HandlerCollection Handlers;
		public Action OnBeforeChanged;
		public Action OnAfterChanged;

		private T internalValue;

		public T Value
		{
			get => internalValue;
			set
			{
				Handlers.InvokeBeforeChanged ();
				OnBeforeChanged?.Invoke ();

				internalValue = value;

				Handlers.InvokeAfterChanged ();
				OnAfterChanged?.Invoke ();
			}
		}

		public EventField ()
		{
			Handlers = new HandlerCollection (this);
		}

		public EventField<B> Watch<B> (Func<T, EventField<B>> chain)
		{
			var watcher = new EventField<B> ();
			Handlers[watcher] += new EventFieldChainHandler<T, B> (this, watcher, chain);
			return watcher;
		}

		public void Dispose ()
		{
			Handlers.Dispose ();
		}
	}
}
