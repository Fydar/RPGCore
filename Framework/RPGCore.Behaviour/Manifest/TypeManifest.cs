using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour.Manifest
{
	public class TypeManifest
	{
		public string Name;
		public string Version;
		public Dictionary<string, JsonValueTypeInformation> JsonTypes;
		public Dictionary<string, JsonObjectTypeInformation> ObjectTypes;

		public static TypeManifest Construct (Type[] valueTypes, Type[] objectTypes)
		{
			var manifest = new TypeManifest ();
			
			var valueTypeInformation = new Dictionary<string, JsonValueTypeInformation> ();
			foreach (var type in valueTypes)
			{
				valueTypeInformation.Add (type.Name, JsonValueTypeInformation.Construct (type));
			}
			manifest.JsonTypes = valueTypeInformation;

			var objectTypeInformation = new Dictionary<string, JsonObjectTypeInformation> ();
			foreach (var type in objectTypes)
			{
				objectTypeInformation.Add (type.Name, JsonObjectTypeInformation.Construct (type));
			}
			manifest.ObjectTypes = objectTypeInformation;

			return manifest;
		}

		public static TypeManifest ConstructBaseTypes ()
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
					typeof(Enum),
					typeof(float),
					typeof(double),
					typeof(decimal),
				},
				new Type[]
				{
					typeof(ExtraData)
				}
			);
		}

		public override string ToString ()
		{
			return JsonConvert.SerializeObject (this, Formatting.Indented);
		}
	}
}
