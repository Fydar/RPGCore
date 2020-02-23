using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public class EventFieldHandlerCollection : IEnumerable<KeyValuePair<object, IEventFieldHandler>>
	{
		public readonly ref struct ContextWrapped
		{
			private readonly IEventField field;
			private readonly object context;

			public ContextWrapped(IEventField field, object context)
			{
				this.field = field;
				this.context = context;
			}

			public void Clear()
			{
				field.Handlers.Clear(context);
			}

			public void Add(IEventFieldHandler handler)
			{
				field.Handlers.internalHandlers.Add(new KeyValuePair<object, IEventFieldHandler>(context, handler));
			}

			public void Remove(IEventFieldHandler handler)
			{
				field.Handlers.internalHandlers.Remove(new KeyValuePair<object, IEventFieldHandler>(context, handler));
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly IEventField field;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<KeyValuePair<object, IEventFieldHandler>> internalHandlers;

		public EventFieldHandlerCollection(IEventField field)
		{
			this.field = field;
			internalHandlers = new List<KeyValuePair<object, IEventFieldHandler>>();
		}

		public ContextWrapped this[object context]
		{
			get => new ContextWrapped(field, context);
		}

		public void Clear(object context)
		{
			for (int i = internalHandlers.Count - 1; i >= 0; i--)
			{
				if (internalHandlers[i].Key == context)
				{
					internalHandlers.RemoveAt(i);
				}
			}
		}

		public void Clear()
		{
			internalHandlers.Clear();
		}

		public void InvokeBeforeChanged()
		{
			if (internalHandlers == null)
			{
				return;
			}

			for (int i = 0; i < internalHandlers.Count; i++)
			{
				internalHandlers[i].Value.OnBeforeChanged();
			}
		}

		public void InvokeAfterChanged()
		{
			if (internalHandlers == null)
			{
				return;
			}

			for (int i = 0; i < internalHandlers.Count; i++)
			{
				internalHandlers[i].Value.OnAfterChanged();
			}
		}

		public IEnumerator<KeyValuePair<object, IEventFieldHandler>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<object, IEventFieldHandler>>)internalHandlers).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<object, IEventFieldHandler>>)internalHandlers).GetEnumerator();
		}
	}
}
