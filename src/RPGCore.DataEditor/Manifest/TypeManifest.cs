using Newtonsoft.Json;
using System.Collections.Generic;

namespace RPGCore.DataEditor.Manifest
{
	public sealed class TypeManifest
	{
		public Dictionary<string, TypeInformation> ObjectTypes;
		public Dictionary<string, NodeInformation> NodeTypes;

		internal TypeManifest()
		{
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
