using System;

namespace RPGCore.Behaviour
{
	public class NodeInformationAttribute : Attribute
	{
		public string Name = "";
		public string Group = "";

		public NodeInformationAttribute (string nodeName)
		{
			Name = nodeName;
		}

		public NodeInformationAttribute (string nodeName, string nodeGroup)
		{
			Name = nodeName;
			Group = nodeGroup;
		}
	}
}