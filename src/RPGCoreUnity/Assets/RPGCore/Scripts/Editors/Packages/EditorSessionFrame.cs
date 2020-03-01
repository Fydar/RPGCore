using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;
using RPGCore.Behaviour.Manifest;
using RPGCore.Demo.BoardGame.Models;
using RPGCore.Demo.Inventory.Nodes;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class EditorSessionFrame : WindowFrame
	{
		public IResource Resource { get; private set;}
		public EditorSession EditorSession { get; private set; }

		private readonly JsonSerializer Serializer = JsonSerializer.Create(new JsonSerializerSettings()
		{
			Converters = new List<JsonConverter>()
			{
				new LocalPropertyIdJsonConverter()
			}
		});

		public EditorSessionFrame(IResource resource)
		{
			Resource = resource;

			JObject editorTarget;
			using (var editorTargetData = Resource.LoadStream())
			using (var sr = new StreamReader(editorTargetData))
			using (var reader = new JsonTextReader(sr))
			{
				editorTarget = JObject.Load(reader);
			}

			var nodes = NodeManifest.Construct(new Type[] {
				typeof (AddNode),
				typeof (RollNode),
				typeof (OutputValueNode),
				typeof (ItemInputNode),
				typeof (ActivatableItemNode),
				typeof (IterateNode),
				typeof (GetStatNode),
			});
			var types = TypeManifest.Construct(
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
					typeof(ExtraData),

					typeof(ResourceTemplate),
					typeof(BuildingTemplate),
				}
			);

			var manifest = new BehaviourManifest()
			{
				Nodes = nodes,
				Types = types,
			};

			string typeName = null;
			if (Resource.Tags.Contains("type-building"))
			{
				typeName = "BuildingTemplate";
			}
			else if (Resource.Tags.Contains("type-resource"))
			{
				typeName = "ResourceTemplate";
			}

			EditorSession = new EditorSession(manifest, editorTarget, typeName, Serializer);
		}

		public override void OnEnable()
		{
		}

		public override void OnGUI()
		{
			if (Resource is ProjectResource projectResource)
			{
				if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(100)))
				{
					using (var file = projectResource.WriteStream())
					{
						Serializer.Serialize(
							new JsonTextWriter(file)
							{
								Formatting = Formatting.Indented
							},
							EditorSession.Instance
						);
					}
				}
			}

			RPGCoreEditor.DrawEditor(EditorSession);
		}
	}
}
