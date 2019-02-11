using System;
using System.Collections.Generic;

namespace Behaviour
{
    public class EventField<T>
	{
		public struct HandlerCollection
		{
			public struct ContextWrapped
			{
				private EventField<T> Field;
				public object Context;
				public IEventFieldHandler Result;
				
				public ContextWrapped(EventField<T> field, object context)
				{
					Field = field;
					Context = context;
					Result = null;
				}

				public void Clear()
				{
					Field.Handlers.Clear(Context);
				}

				public static ContextWrapped operator +(ContextWrapped left, IEventFieldHandler right)
				{
					left.Result = right;
					return left;
				}
			}
			private EventField<T> field;
			private List<KeyValuePair<object, IEventFieldHandler>> handlers;

			public HandlerCollection(EventField<T> field)
			{
				this.field = field;
				handlers = null;
			}

			public ContextWrapped this[object context]
			{
				get {
					return new ContextWrapped(field, context);
				}
				set {
					if(handlers == null)
						handlers = new List<KeyValuePair<object, IEventFieldHandler>>();

					handlers.Add (new KeyValuePair<object, IEventFieldHandler>(context, value.Result));
				}
			}

			public void Clear(object context)
			{
				for (int i = handlers.Count - 1; i >= 0 ; i--)
				{
					if(handlers[i].Key == context)
						handlers.RemoveAt(i);
				}
			}

			public void InvokeBeforeChanged()
			{
				if (handlers == null)
					return;

				foreach(var handler in handlers)
				{
					handler.Value.OnBeforeChanged();
				}
			}

			public void InvokeAfterChanged()
			{
				if (handlers == null)
					return;

				foreach(var handler in handlers)
				{
					handler.Value.OnAfterChanged();
				}
			}
		}
		public HandlerCollection Handlers;
		public Action OnBeforeChanged;
		public Action OnAfterChanged;

		private T internalValue;

		public T Value
		{
			get
			{
				return internalValue;
			}
			set
			{
				Handlers.InvokeBeforeChanged();
				if (OnBeforeChanged != null)
					OnBeforeChanged();

				internalValue = value;

				Handlers.InvokeAfterChanged();
				if (OnAfterChanged != null)
					OnAfterChanged();
			}
		}

		public EventField()
		{
			Handlers = new HandlerCollection(this);
		}

		public EventField<B> Watch<B>(Func<T, EventField<B>> chain)
		{
			var watcher = new EventField<B>();
			Handlers[this] += new EventFieldChainHandler<T, B>(this, watcher, chain);
			return watcher;
		}
    }
}
