using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Behaviour.Manifest
{
	public class NodeManifest
	{
		public string Name;
		public string Version;
		public NodeInformation[] Nodes;

		public static NodeManifest Construct ()
		{
			var manifest = new NodeManifest ();

			var information = new List<NodeInformation> ();
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies ())
			{
				foreach (var type in assembly.GetTypes ())
				{
					if (type.IsAbstract)
						continue;

					if (typeof (Node).IsAssignableFrom (type))
					{
						information.Add (NodeInformation.Construct (type));
					}
				}
			}
			manifest.Nodes = information.ToArray ();
			return manifest;
		}

		public static NodeManifest Construct (Type[] types)
		{
			var manifest = new NodeManifest ();

			var information = new List<NodeInformation> ();
			foreach (var type in types)
			{
				information.Add (NodeInformation.Construct (type));
			}
			manifest.Nodes = information.ToArray ();
			return manifest;
		}

		public override string ToString ()
		{
			return JsonConvert.SerializeObject (this, Formatting.Indented);
		}
	}
}
