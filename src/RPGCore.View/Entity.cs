using RPGCore.Behaviour;
using System;
using System.Collections.Generic;

namespace RPGCore.View
{
	public abstract class Entity
	{
		public EntityRef Id;

		public abstract IEnumerable<KeyValuePair<string, IEventField>> SyncedObjects { get; }
	}
}
