using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.Behaviour
{
	public class EventFieldHandlerCollection : IEnumerable<KeyValuePair<object, IEventFieldHandler>>, IDisposable
	{
		public readonly ref struct ContextWrapped
		{
			private readonly IEventField Field;
			private readonly object Context;

			public ContextWrapped(IEventField field, object context)
			{
				Field = field;
				Context = context;
			}

			public void Clear()
			{
				Field.Handlers.Clear (Context);
			}

			public void Add(IEventFieldHandler handler)
			{
				Field.Handlers.InternalHandlers.Add (new KeyValuePair<object, IEventFieldHandler> (Context, handler));
			}

			public void Remove(IEventFieldHandler handler)
			{
				Field.Handlers.InternalHandlers.Remove (new KeyValuePair<object, IEventFieldHandler> (Context, handler));
			}
		}

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private readonly IEventField Field;

		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private readonly List<KeyValuePair<object, IEventFieldHandler>> InternalHandlers;

		public EventFieldHandlerCollection(IEventField field)
		{
			Field = field;
			InternalHandlers = new List<KeyValuePair<object, IEventFieldHandler>> ();
		}

		public ContextWrapped this[object context]
		{
			get => new ContextWrapped (Field, context);
		}

		public void Clear(object context)
		{
			for (int i = InternalHandlers.Count - 1; i >= 0; i--)
			{
				if (InternalHandlers[i].Key == context)
				{
					InternalHandlers.RemoveAt (i);
				}
			}
		}

		public void Clear()
		{
			InternalHandlers.Clear ();
		}

		public void InvokeBeforeChanged()
		{
			if (InternalHandlers == null)
			{
				return;
			}

			for (int i = 0; i < InternalHandlers.Count; i++)
			{
				InternalHandlers[i].Value.OnBeforeChanged ();
			}
		}

		public void InvokeAfterChanged()
		{
			if (InternalHandlers == null)
			{
				return;
			}

			for (int i = 0; i < InternalHandlers.Count; i++)
			{
				InternalHandlers[i].Value.OnAfterChanged ();
			}
		}

		public void Dispose()
		{
			if (InternalHandlers == null)
			{
				return;
			}

			for (int i = 0; i < InternalHandlers.Count; i++)
			{
				InternalHandlers[i].Value.Dispose ();
			}
		}

		public IEnumerator<KeyValuePair<object, IEventFieldHandler>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<object, IEventFieldHandler>>)InternalHandlers).GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<object, IEventFieldHandler>>)InternalHandlers).GetEnumerator ();
		}
	}
}
