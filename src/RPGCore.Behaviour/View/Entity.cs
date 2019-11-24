using Newtonsoft.Json;
using RPGCore.Behaviour;
using System.Collections.Generic;

namespace RPGCore.View
{
	public abstract class Entity
	{
		[JsonIgnore]
		public EntityRef Id;

		[JsonIgnore]
		public abstract IEnumerable<KeyValuePair<string, ISyncField>> SyncedObjects { get; }
	}
}
