using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Events;

public sealed class EventFieldHandlerCollection : IEnumerable<KeyValuePair<object, IEventFieldHandler>>
{
	public readonly ref struct ContextWrapped
	{
		private readonly IReadOnlyEventField field;
		private readonly object context;

		public ContextWrapped(IReadOnlyEventField field, object context)
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

			if (handler is IHandlerUsedCallback usedCallback)
			{
				usedCallback.OnUse(field);
			}
		}

		public void AddAndInvoke(IEventFieldHandler handler)
		{
			Add(handler);
			handler.OnAfterChanged();
		}

		public void Remove(IEventFieldHandler handler)
		{
			field.Handlers.internalHandlers.Remove(new KeyValuePair<object, IEventFieldHandler>(context, handler));
		}

		public void InvokeAndRemove(IEventFieldHandler handler)
		{
			Remove(handler);
			handler.OnBeforeChanged();
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly IReadOnlyEventField field;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly List<KeyValuePair<object, IEventFieldHandler>> internalHandlers;

	public ContextWrapped this[object context] => new ContextWrapped(field, context);

	public EventFieldHandlerCollection(IReadOnlyEventField field)
	{
		this.field = field;
		internalHandlers = new List<KeyValuePair<object, IEventFieldHandler>>();
	}

	public void Clear(object context)
	{
		for (int i = internalHandlers.Count - 1; i >= 0; i--)
		{
			if (internalHandlers[i].Key == context)
			{
				internalHandlers[i].Value.OnBeforeChanged();
				internalHandlers.RemoveAt(i);
			}
		}
	}

	public void Clear()
	{
		internalHandlers.Clear();
	}

	public void InvokeAndClear()
	{
		InvokeBeforeChanged();
		internalHandlers.Clear();
	}

	public void InvokeBeforeChanged()
	{
		for (int i = 0; i < internalHandlers.Count; i++)
		{
			internalHandlers[i].Value.OnBeforeChanged();
		}
	}

	public void InvokeAfterChanged()
	{
		for (int i = 0; i < internalHandlers.Count; i++)
		{
			internalHandlers[i].Value.OnAfterChanged();
		}
	}

	public IEnumerator<KeyValuePair<object, IEventFieldHandler>> GetEnumerator()
	{
		return internalHandlers.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return internalHandlers.GetEnumerator();
	}
}
