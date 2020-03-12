using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;
using RPGCore.Behaviour.Manifest;
using RPGCore.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class EditorSessionFrame : WindowFrame
	{
		public IResource Resource { get; private set; }
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

			var manifest = BehaviourManifest.CreateFromAppDomain(AppDomain.CurrentDomain);

			string typeName = null;
			if (Resource.Tags.Contains("type-building"))
			{
				typeName = "BuildingTemplate";
			}
			else if (Resource.Tags.Contains("type-resource"))
			{
				typeName = "ResourceTemplate";
			}
			else if (Resource.Tags.Contains("type-buildingpack"))
			{
				typeName = "BuildingPackTemplate";
			}
			else
			{
				typeName = "ProceduralItemTemplate";
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
				EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
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

				if (GUILayout.Button("View Manifest", EditorStyles.toolbarButton, GUILayout.Width(120)))
				{
					GenericWindow.Open(new GUIContent("Manifest"), new JsonTextWindowFrame(EditorSession.Manifest.ToString()));
				}
				EditorGUILayout.EndHorizontal();
			}

			RPGCoreEditor.DrawEditor(EditorSession);
		}
	}
}
