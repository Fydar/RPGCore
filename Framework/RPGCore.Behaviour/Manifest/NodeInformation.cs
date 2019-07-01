using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Behaviour.Manifest
{
	public class NodeInformation : JsonObjectTypeInformation
	{
		public Dictionary<string, SocketInformation> Inputs;
		public Dictionary<string, SocketInformation> Outputs;

		public static NodeInformation Construct (Type nodeType)
		{
			var nodeInformation = new NodeInformation();

			var typeDefinition = new Type[] { typeof (IGraphInstance), nodeType.BaseType.GenericTypeArguments[0] };
			var inputsProperty = nodeType.GetMethod ("Inputs", typeDefinition);
			var outputsProperty = nodeType.GetMethod ("Outputs", typeDefinition);

			var nodeTemplate = (Node)Activator.CreateInstance (nodeType);
			object metadataInstance = Activator.CreateInstance (typeDefinition[1]);
			var singleNodeGraph = new ManifestCaptureGraphInstance (nodeTemplate);

			int inputId = 0;
			int outputId = 0;
			var inputSocketFields = new List<FieldInfo> ();
			var outputSocketFields = new List<FieldInfo> ();
			var fieldInfos = new Dictionary<string, FieldInformation> ();
			foreach (var field in nodeType.GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				if (field.FieldType == typeof (OutputSocket))
				{
					field.SetValue (nodeTemplate, new OutputSocket (outputId));
					outputId++;
					outputSocketFields.Add (field);
				}
				else
				{
					if (field.FieldType == typeof (InputSocket))
					{
						field.SetValue (nodeTemplate, new InputSocket (inputId));
						inputId++;
						inputSocketFields.Add (field);
					}
					fieldInfos.Add (field.Name, FieldInformation.Construct (field, nodeTemplate));
				}
			}
			nodeInformation.Fields = fieldInfos;

			object[] connectParameters = { singleNodeGraph, metadataInstance };
			var inputsArray = (InputMap[])inputsProperty.Invoke (nodeTemplate, connectParameters);
			var outputsArray = (OutputMap[])outputsProperty.Invoke (nodeTemplate, connectParameters);

			if (inputsArray != null)
			{
				nodeInformation.Inputs = new Dictionary<string, SocketInformation>(inputsArray.Length);

				for (int i = 0; i < inputsArray.Length; i++)
				{
					var map = inputsArray[i];
					var field = inputSocketFields[map.Source.TargetId];
					nodeInformation.Inputs.Add(field.Name, SocketInformation.Construct (field, map));
				}
			}
			else
			{
				nodeInformation.Inputs = null;
			}

			if (outputsArray != null)
			{
				nodeInformation.Outputs = new Dictionary<string, SocketInformation>(outputsArray.Length);

				for (int i = 0; i < outputsArray.Length; i++)
				{
					var map = outputsArray[i];
					var field = outputSocketFields[map.Source.Id];
					nodeInformation.Outputs.Add(field.Name, SocketInformation.Construct (field, map));
				}
			}
			else
			{
				nodeInformation.Outputs = null;
			}

			return nodeInformation;
		}
	}
}
