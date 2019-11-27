using Newtonsoft.Json;
using RPGCore.Behaviour;
using RPGCore.Traits;
using System.Collections.Generic;

namespace RPGCore.View
{
	public class ViewCharacter : Entity
	{
		public TraitCollection Traits;

		public EventField<string> Name = new EventField<string> ();
		public EventField<EntityRef> SelectedTarget = new EventField<EntityRef> ();

		public ViewCharacter()
		{
			Id = new EntityRef ()
			{
				EntityId = LocalId.NewId ()
			};
		}

		[JsonIgnore]
		public override IEnumerable<KeyValuePair<string, ISyncField>> SyncedObjects
		{
			get
			{
				yield return new KeyValuePair<string, ISyncField> ("name", Name);
				yield return new KeyValuePair<string, ISyncField> ("selectedTarget", SelectedTarget);
				yield return new KeyValuePair<string, ISyncField> ("traits", Traits);
			}
		}

		public override string ToString()
		{
			return $"{Name.Value}({Id}, {string.Join (", ", Traits.States)}";
		}
	}
}
