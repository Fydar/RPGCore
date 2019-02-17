using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour.Manifest
{
	public class TypeManifest
	{
		public string Name;
		public string Version;
		public TypeInformation[] Types;

		public static TypeManifest Construct (Type[] types)
		{
			var manifest = new TypeManifest ();

			var information = new List<TypeInformation> ();
			foreach (var type in types)
			{
				information.Add (TypeInformation.Construct (type));
			}
			manifest.Types = information.ToArray ();
			return manifest;
		}

		public static TypeManifest ConstructBaseTypes ()
		{
			return Construct (new Type[]
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
			});
		}

		public override string ToString ()
		{
			return JsonConvert.SerializeObject (this, Formatting.Indented);
		}
	}
}
