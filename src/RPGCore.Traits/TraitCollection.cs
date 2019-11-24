using Newtonsoft.Json;
using RPGCore.Behaviour;
using RPGCore.View;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RPGCore.Traits
{
	/// <summary>
	/// Contains a collection for all stats belonging to a player or an item.
	/// </summary>
	[JsonObject]
	public class TraitCollection : IEventCollection<string, ISyncField>, ISyncField
	{
		[JsonProperty]
		public EventCollection<StatIdentifier, StatInstance> Stats;
		[JsonProperty]
		public EventCollection<StateIdentifier, StateInstance> States;

		public StatInstance this[StatIdentifier identifier]
		{
			get
			{
				if (Stats == null)
				{
					return null;
				}

				return Stats[identifier];
			}
		}

		public StateInstance this[StateIdentifier identifier]
		{
			get
			{
				if (Stats == null)
				{
					return null;
				}

				return States[identifier];
			}
		}

		ISyncField IEventCollection<string, ISyncField>.this[string key]
		{
			get
			{
				if (key == nameof (Stats))
				{
					return Stats;
				}
				else if (key == nameof (States))
				{
					return States;
				}
				return null;
			}
		}

		#region NETWORK_EVENT_MODEL
		EventCollectionHandlerCollection<string, ISyncField> IEventCollection<string, ISyncField>.Handlers { get; }

		public void Apply(ViewPacket packet)
		{
			string key = packet.FieldPath.Split ('.').Last ();

			switch (packet.PacketType)
			{
				case ViewPacket.ViewPacketType.AddCollectionItem:

					if (key == nameof (Stats))
					{
						((IEventCollection<string, ISyncField>)this).Add (key, packet.Data.ToObject<StatInstance> ());
					}
					else if (key == nameof (States))
					{
						((IEventCollection<string, ISyncField>)this).Add (key, packet.Data.ToObject<StateInstance> ());
					}
					break;

				case ViewPacket.ViewPacketType.RemoveCollectionItem:

					((IEventCollection<string, ISyncField>)this).Remove (key);

					break;

			}
		}

		void IEventCollection<string, ISyncField>.Add(string key, ISyncField value)
		{
			if (key == nameof (Stats))
			{
				Stats = (EventCollection<StatIdentifier, StatInstance>)value;
			}
			else if (key == nameof (States))
			{
				States = (EventCollection<StateIdentifier, StateInstance>)value;
			}
		}

		object ISyncField.AddSyncHandler(ViewDispatcher viewDispatcher, EntityRef root, string path)
		{
			return new SyncEventCollectionHandler<string, ISyncField> (viewDispatcher, root, path);
		}

		bool IEventCollection<string, ISyncField>.ContainsKey(string key)
		{
			return key == nameof (Stats) || key == nameof (States);
		}

		IEnumerator<KeyValuePair<string, ISyncField>> IEnumerable<KeyValuePair<string, ISyncField>>.GetEnumerator()
		{
			yield return new KeyValuePair<string, ISyncField> (nameof (Stats), Stats);
			yield return new KeyValuePair<string, ISyncField> (nameof (States), States);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			yield return new KeyValuePair<string, ISyncField> (nameof (Stats), Stats);
			yield return new KeyValuePair<string, ISyncField> (nameof (States), States);
		}

		bool IEventCollection<string, ISyncField>.Remove(string key)
		{
			if (key == nameof (Stats))
			{
				Stats = null;
				return true;
			}
			else if (key == nameof (States))
			{
				States = null;
				return true;
			}
			return false;
		}

		bool IEventCollection<string, ISyncField>.TryGetValue(string key, out ISyncField value)
		{
			if (key == nameof (Stats))
			{
				value = Stats;
				return true;
			}
			else if (key == nameof (States))
			{
				value = States;
				return true;
			}
			value = null;
			return false;
		}
		#endregion
	}
}
