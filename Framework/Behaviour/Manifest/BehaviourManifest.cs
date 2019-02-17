using Newtonsoft.Json;

namespace Behaviour.Manifest
{
    public class BehaviourManifest
    {
        public TypeManifest Types;
        public NodeManifest Nodes;

		public override string ToString ()
		{
			return JsonConvert.SerializeObject (this, Formatting.Indented);
		}
    }
}