using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour.Manifest
{
	public sealed class TypeManifest
	{
		public string Name;
		public string Version;
		public Dictionary<string, TypeInformation> JsonTypes;
		public Dictionary<string, TypeInformation> ObjectTypes;

		public static TypeManifest Construct(Type[] valueTypes, Type[] objectTypes)
		{
			var manifest = new TypeManifest ();

			var valueTypeInformation = new Dictionary<string, TypeInformation> ();
			foreach (var type in valueTypes)
			{
				valueTypeInformation.Add (type.Name, TypeInformation.Construct (type));
			}
			manifest.JsonTypes = valueTypeInformation;

			var objectTypeInformation = new Dictionary<string, TypeInformation> ();
			foreach (var type in objectTypes)
			{
				objectTypeInformation.Add (type.Name, TypeInformation.Construct (type));
			}
			manifest.ObjectTypes = objectTypeInformation;

			return manifest;
		}

		public static TypeManifest ConstructBaseTypes()
		{
			return Construct (
				new Type[]
				{
					typeof(bool),
					typeof(string),
					typeof(int),
					typeof(byte),
					typeof(long),
					typeof(short),
					typeof(uint),
					typeof(ulong),
					typeof(ushort),
					typeof(sbyte),
					typeof(char),
					typeof(float),
					typeof(double),
					typeof(decimal),
					typeof(InputSocket),
					typeof(LocalId),
				},
				new Type[]
				{
					typeof(SerializedGraph),
					typeof(SerializedNode),
					typeof(PackageNodeEditor),
					typeof(PackageNodePosition),
				}
			);
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject (this, Formatting.Indented);
		}
	}
}
