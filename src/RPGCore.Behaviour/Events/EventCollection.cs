using Newtonsoft.Json;
using RPGCore.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Behaviour
{
	[JsonObject]
	public class EventCollection<TKey, TValue> : IEventCollection<TKey, TValue>, IDisposable, ISyncField
	{
		[JsonIgnore]
		public EventCollectionHandlerCollection<TKey, TValue> Handlers { get; set; }

		private readonly Dictionary<TKey, TValue> Collection;

		public TValue this[TKey key] => Collection[key];

		public EventCollection()
		{
			Handlers = new EventCollectionHandlerCollection<TKey, TValue> (this);
			Collection = new Dictionary<TKey, TValue> ();
		}

		public void Dispose()
		{
			Handlers.Dispose ();
		}

		public bool ContainsKey(TKey key)
		{
			return Collection.ContainsKey (key);
		}

		public void Add(TKey key, TValue value)
		{
			Collection.Add (key, value);

			Handlers.InvokeAdd (key, value);
		}

		public bool Remove(TKey key)
		{
			if (!Collection.TryGetValue (key, out var eventObject))
			{
				return false;
			}

			bool result = Collection.Remove (key);
			if (result)
			{
				Handlers.InvokeRemoved (key, eventObject);
			}

			return result;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return Collection.TryGetValue (key, out value);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return Collection.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Collection.GetEnumerator ();
		}

		public object AddSyncHandler(ViewDispatcher viewDispatcher, EntityRef root, string path)
		{
			var handler = new SyncEventCollectionHandler<TKey, TValue> (viewDispatcher, root, path);
			Handlers[this].Add (handler);
			return handler;
		}

		public void Apply(ViewPacket packet)
		{
			switch (packet.PacketType)
			{
				case ViewPacket.ViewPacketType.AddCollectionItem:

					Add ((TKey)(object)packet.FieldPath.Split ('.').Last (), packet.Data.ToObject<TValue> ());

					break;

				case ViewPacket.ViewPacketType.RemoveCollectionItem:

					Remove ((TKey)(object)packet.FieldPath.Split ('.').Last ());

					break;

			}
		}
	}
}
