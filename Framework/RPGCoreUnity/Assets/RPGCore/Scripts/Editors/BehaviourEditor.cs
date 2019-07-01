using UnityEngine;
using UnityEditor;
using RPGCore.Packages;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Manifest;
using RPGCore.Behaviour.Editor;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;

namespace RPGCore.Unity.Editors
{
	public class BehaviourEditor : EditorWindow
	{
		public ProjectImport CurrentPackage;

		public bool HasCurrentResource;
		public ProjectResource CurrentResource;
		public SerializedGraph editorTarget;
		public List<EditorObject> editors;

		private JsonSerializer serializer = new JsonSerializer();

		[MenuItem("Window/Behaviour")]
		public static void Open()
		{
			var window = EditorWindow.GetWindow<BehaviourEditor>();

			window.Show();
		}

		public void OnGUI()
		{
			CurrentPackage = (ProjectImport)EditorGUILayout.ObjectField(CurrentPackage, typeof(ProjectImport), true);

			var explorer = CurrentPackage.Explorer;

			foreach (var resource in explorer.Resources)
			{
				if (!resource.Name.EndsWith(".bhvr"))
				{
					continue;
				}

				if (GUILayout.Button(resource.ToString()))
				{
					CurrentResource = resource;
					HasCurrentResource = true;
					editors = null;
				}
			}

			if (HasCurrentResource && CurrentResource != null)
			{
				if (editors == null)
				{
					Debug.Log(CurrentResource);

					var editorTargetData = CurrentResource.LoadStream();

					using (var sr = new StreamReader(editorTargetData))
					using (var reader = new JsonTextReader(sr))
					{
						editorTarget = serializer.Deserialize<SerializedGraph>(reader);
					}

					var nodes = NodeManifest.Construct(new Type[] { typeof(AddNode), typeof(RollNode) });
					var types = TypeManifest.ConstructBaseTypes();

					var manifest = new BehaviourManifest()
					{
						Nodes = nodes,
						Types = types
					};

					editors = new List<EditorObject>();
					foreach (var editNode in editorTarget.Nodes)
					{
						var typeData = manifest.Nodes.Nodes[editNode.Value.Type];

						editors.Add(new EditorObject(typeData, editNode.Value.Data));
					}
				}

				if (GUILayout.Button("Save"))
				{
					using (var file = CurrentResource.WriteStream())
					{
						serializer.Serialize(new JsonTextWriter(file)
						{ Formatting = Formatting.Indented }, editorTarget);
					}
				}

				foreach (var editor in editors)
				{
					
					EditorGUILayout.BeginVertical(EditorStyles.helpBox);
					foreach (var field in editor)
					{
						if (field.Information.Type == "Int32")
						{
							EditorGUI.BeginChangeCheck();
							int newValue = EditorGUILayout.IntField(field.Information.Name, (int)field.Property.Value);
							if (EditorGUI.EndChangeCheck())
							{
								field.Property.Value = newValue;
							}
						}
						else if (field.Information.Type == "String")
						{
							EditorGUI.BeginChangeCheck();
							string newValue = EditorGUILayout.TextField(field.Information.Name, (string)field.Property.Value);
							if (EditorGUI.EndChangeCheck())
							{
								field.Property.Value = newValue;
							}
						}
						else
						{
							EditorGUILayout.LabelField(field.Information.Name);
							EditorGUILayout.TextArea(field.Property.Value.ToString());
						}
					}
					EditorGUILayout.EndVertical();
				}

			}
		}
	}
}