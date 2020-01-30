using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Behaviour.Manifest
{
	public sealed class NodeInformation : TypeInformation
	{
		public Dictionary<string, SocketInformation> Inputs;
		public Dictionary<string, SocketInformation> Outputs;

		public static NodeInformation ConstructNodeInformation(Type nodeType)
		{
			var nodeInformation = new NodeInformation ();

			var nodeTemplate = (NodeTemplate)Activator.CreateInstance (nodeType);
			var metadataInstance = (NodeInstanceBase)nodeTemplate.CreateInstance ();

			var instanceType = metadataInstance.GetType ();

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
					fieldInfos.Add (field.Name, FieldInformation.ConstructFieldInformation (field, nodeTemplate));
				}
			}
			nodeInformation.Fields = fieldInfos;

			var nodeProperty = instanceType.GetProperty ("Template");
			nodeProperty.SetValue (metadataInstance, nodeTemplate);

			var connectionMapper = new ConnectionMapper (metadataInstance, singleNodeGraph);

			var inputsArray = metadataInstance.Inputs (connectionMapper);
			var outputsArray = metadataInstance.Outputs (connectionMapper);

			if (inputsArray != null)
			{
				nodeInformation.Inputs = new Dictionary<string, SocketInformation> (inputsArray.Length);

				for (int i = 0; i < inputsArray.Length; i++)
				{
					var map = inputsArray[i];
					var field = inputSocketFields[map.ConnectionId];
					nodeInformation.Inputs.Add (field.Name, SocketInformation.Construct (field, map));
				}
			}
			else
			{
				nodeInformation.Inputs = null;
			}

			if (outputsArray != null)
			{
				nodeInformation.Outputs = new Dictionary<string, SocketInformation> (outputsArray.Length);

				for (int i = 0; i < outputsArray.Length; i++)
				{
					var map = outputsArray[i];
					var field = outputSocketFields[map.ConnectionId];
					nodeInformation.Outputs.Add (field.Name, SocketInformation.Construct (field, map));
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
