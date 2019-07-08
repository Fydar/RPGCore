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
using Newtonsoft.Json.Linq;

namespace RPGCore.Unity.Editors
{
	public class BehaviourEditor : EditorWindow
	{
		public ProjectImport CurrentPackage;

		public bool HasCurrentResource;
		public bool HasEditor;
		public ProjectResource CurrentResource;
		public JObject editorTarget;
		public EditorObject graphEditor;

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
					HasEditor = false;
				}
			}

			if (HasCurrentResource && CurrentResource != null)
			{
				if (HasEditor == false)
				{
					Debug.Log(CurrentResource);

					var editorTargetData = CurrentResource.LoadStream();

					using (var sr = new StreamReader(editorTargetData))
					using (var reader = new JsonTextReader(sr))
					{
						editorTarget = JObject.Load(reader);
						// editorTarget = serializer.Deserialize(reader);
					}

					var nodes = NodeManifest.Construct(new Type[] { typeof(AddNode), typeof(RollNode) });
					var types = TypeManifest.ConstructBaseTypes();

					var manifest = new BehaviourManifest()
					{
						Nodes = nodes,
						Types = types
					};


					graphEditor = new EditorObject(manifest, manifest.Types.ObjectTypes["SerializedGraph"], editorTarget);
					HasEditor = true;
				}

				if (GUILayout.Button("Save"))
				{
					using (var file = CurrentResource.WriteStream())
					{
						serializer.Serialize(new JsonTextWriter(file)
						{ Formatting = Formatting.Indented }, editorTarget);
					}
				}

				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				DrawEditor(graphEditor);
				EditorGUILayout.EndVertical();
			}
		}

		public static void DrawEditor(EditorObject editor)
		{
			foreach (var field in editor)
			{				
				if (field.Info.Type == "Int32")
				{
					EditorGUI.BeginChangeCheck();
					int newValue = EditorGUILayout.IntField(field.Name, field.JsonValue.ToObject<int>());
					if (EditorGUI.EndChangeCheck())
					{
						field.JsonValue.Value = newValue;
					}
				}
				else if (field.Info.Type == "String")
				{
					EditorGUI.BeginChangeCheck();
					string newValue = EditorGUILayout.TextField(field.Name, field.JsonValue.ToObject<string>());
					if (EditorGUI.EndChangeCheck())
					{
						field.JsonValue.Value = newValue;
					}
				}
				else if (field.Info.Type == "Boolean")
				{
					EditorGUI.BeginChangeCheck();
					bool newValue = EditorGUILayout.Toggle(field.Name, field.JsonValue.ToObject<bool>());
					if (EditorGUI.EndChangeCheck())
					{
						field.JsonValue.Value = newValue;
					}
				}
				else if (field.Info.Type == "InputSocket")
				{
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.LabelField(field.Name, field.JsonValue.ToObject<string>());
					if (EditorGUI.EndChangeCheck())
					{
						//field.JsonValue.Value = newValue;
					}
				}
				else if (field.ObjectTypeInfo != null)
				{
					EditorGUILayout.LabelField(field.Name);

					EditorGUI.indentLevel++;

					var objectEditor = new EditorObject(editor.Manifest, field.ObjectTypeInfo, field.JsonObject);
					DrawEditor(objectEditor);
					
					EditorGUI.indentLevel--;
				}
				else
				{
					EditorGUILayout.LabelField(field.Name, "Unknown Type");
				}
			}
		}
	}
}