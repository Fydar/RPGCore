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
			if (Types.JsonTypes.TryGetValue (type, out var jsonType))
			{
				return jsonType;
			}
			if (Types.ObjectTypes.TryGetValue (type, out var objectType))
			{
				return objectType;
			}
			if (Nodes.Nodes.TryGetValue (type, out var nodeType))
			{
				return nodeType;
			}
			return null;
		}
	}
}
