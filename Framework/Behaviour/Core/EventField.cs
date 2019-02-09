using System;
using System.Collections.Generic;

namespace Behaviour
{
	public interface IEventFieldHandler
	{
		object Source { get; set; }
		void OnBeforeChanged();
		void OnAfterChanged();
	}

	public struct EventFieldMirrorHandler<T> : IEventFieldHandler
	{
		public object Source { get; set; }
		public EventField<T> SourceField;
		public EventField<T> Target;

		public EventFieldMirrorHandler(EventField<T> source, EventField<T> target)
		{
			Source = null;
			SourceField = source;
			Target = target;
		}

		public void OnBeforeChanged()
		{
			Target.Value = default(T);
		}

		public void OnAfterChanged()
		{
			Target.Value = SourceField.Value;
		}
	}
	
	public class EventField<T>
	{
		public struct HandlerCollection
		{
			public struct ContextWrapped
			{
				public object Context;
				public IEventFieldHandler Result;
				
				public ContextWrapped(ref HandlerCollection handlerCollection, object context)
				{
					Context = context;
					Result = null;
				}

				public void Clear()
				{
					
				}

				public static ContextWrapped operator +(ContextWrapped left, IEventFieldHandler right)
				{
					left.Result = right;
					return left;
				}
			}
			private EventField<T> field;
			public List<IEventFieldHandler> handlers;

			public HandlerCollection(EventField<T> field)
			{
				this.field = field;
				handlers = null;
			}

			public ContextWrapped this[object context]
			{
				get {
					return new ContextWrapped(ref this, context);
				}
				set {
					if(handlers == null)
						handlers = new List<IEventFieldHandler>();

					handlers.Add (value.Result);
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
				if(Handlers.handlers != null)
				{
					foreach(var handler in Handlers.handlers)
					{
						handler.OnBeforeChanged();
					}
				}
				if (OnBeforeChanged != null)
					OnBeforeChanged();

				internalValue = value;

				if(Handlers.handlers != null)
				{
					foreach(var handler in Handlers.handlers)
					{
						handler.OnAfterChanged();
					}
				}
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
			OnAfterChanged += () => {
				var target = chain (Value);
				if(target == null)
					return;
					
				target.OnAfterChanged += () => {
					watcher.Value = target.Value;
				};
				watcher.Value = target.Value;
			};
			return watcher;
		}
	}
}
