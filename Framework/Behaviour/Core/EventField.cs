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
			private EventField<T> field;

			public HandlerCollection(EventField<T> field)
			{
				this.field = field;
			}
		}
		public HandlerCollection Handlers;
		public Action OnBeforeChanged;
		public Action OnAfterChanged;
		public List<IEventFieldHandler> handlers = new List<IEventFieldHandler>();

		private T internalValue;

		public T Value
		{
			get
			{
				return internalValue;
			}
			set
			{
				if(handlers != null)
				{
					foreach(var handler in handlers)
					{
						handler.OnBeforeChanged();
					}
				}
				if (OnBeforeChanged != null)
					OnBeforeChanged();

				internalValue = value;

				if(handlers != null)
				{
					foreach(var handler in handlers)
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
