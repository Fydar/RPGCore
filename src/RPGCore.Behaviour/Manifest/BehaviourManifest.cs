using Newtonsoft.Json;

namespace RPGCore.Behaviour.Manifest
{
	public sealed class BehaviourManifest
	{
		public TypeManifest Types;
		public NodeManifest Nodes;

		public override string ToString ()
		{
			return JsonConvert.SerializeObject (this, Formatting.Indented);
		}

		public TypeInformation GetTypeInformation (string type)
		{
			string lookupType;
			int arrayIndex = type.LastIndexOf ('[');
			if (arrayIndex == -1)
			{
				lookupType = type;
			}
			else
			{
				lookupType = type.Substring (0, arrayIndex);
			}

			if (Types.JsonTypes.TryGetValue (lookupType, out var jsonType))
			{
				return jsonType;
			}
			if (Types.ObjectTypes.TryGetValue (lookupType, out var objectType))
			{
				return objectType;
			}
			if (Nodes.Nodes.TryGetValue (lookupType, out var nodeType))
			{
				return nodeType;
			}
			return null;
		}
	}
}
