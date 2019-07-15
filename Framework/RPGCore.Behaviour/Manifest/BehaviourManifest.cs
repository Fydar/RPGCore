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
	}
}
