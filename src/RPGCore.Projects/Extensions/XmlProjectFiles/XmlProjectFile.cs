using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace RPGCore.Projects
{
	internal static class XmlProjectFile
	{
		public static void Format(XmlDocument document)
		{
			Minify(document);

			var nodes = AllNodesWithIndent(document.DocumentElement, 1).ToArray();

			document.DocumentElement.InsertBefore(document.CreateWhitespace("\n\n"), document.DocumentElement.FirstChild);

			foreach (var indentedNode in nodes)
			{
				var childNode = indentedNode.Item2;

				if (childNode.NodeType == XmlNodeType.Element
					|| childNode.NodeType == XmlNodeType.Comment)
				{
					int indent = indentedNode.Item1;

					// Indent opening tag
					var startIndent = document.CreateWhitespace(new string(' ', indent * 2));
					childNode.ParentNode.InsertBefore(startIndent, childNode);

					if (HasChildElement(childNode))
					{
						childNode.ParentNode.InsertAfter(document.CreateWhitespace("\n\n"), childNode);

						childNode.InsertBefore(document.CreateWhitespace("\n"), childNode.FirstChild);

						if (HasChildObjects(childNode))
						{
							childNode.InsertBefore(document.CreateWhitespace("\n"), childNode.FirstChild);
						}

						// Indent closing tag
						var endIndent = document.CreateWhitespace(new string(' ', indent * 2));
						childNode.InsertAfter(endIndent, childNode.LastChild);
					}
					else
					{
						childNode.ParentNode.InsertAfter(document.CreateWhitespace("\n"), childNode);
					}
				}
			}
		}

		public static void Minify(XmlDocument document)
		{
			var allNodes = AllNodes(document.DocumentElement).ToArray();
			foreach (var node in allNodes)
			{
				if (node.NodeType == XmlNodeType.SignificantWhitespace
				|| node.NodeType == XmlNodeType.Whitespace)
				{
					node.ParentNode.RemoveChild(node);
				}
			}
		}

		private static bool HasChildElement(XmlNode node)
		{
			for (int i = 0; i < node.ChildNodes.Count; i++)
			{
				var childNode = node.ChildNodes[i];

				if (childNode.NodeType == XmlNodeType.Element)
				{
					return true;
				}
			}
			return false;
		}

		private static bool HasChildObjects(XmlNode node)
		{
			for (int i = 0; i < node.ChildNodes.Count; i++)
			{
				var childNode = node.ChildNodes[i];

				if (HasChildElement(childNode))
				{
					return true;
				}
			}
			return false;
		}

		private static IEnumerable<XmlNode> AllNodes(XmlNode rootNode)
		{
			for (int i = 0; i < rootNode.ChildNodes.Count; i++)
			{
				var childNode = rootNode.ChildNodes[i];
				yield return childNode;

				foreach (var node in AllNodes(childNode))
				{
					yield return node;
				}
			}
		}

		private static IEnumerable<Tuple<int, XmlNode>> AllNodesWithIndent(XmlNode rootNode, int indent = 0)
		{
			for (int i = 0; i < rootNode.ChildNodes.Count; i++)
			{
				var childNode = rootNode.ChildNodes[i];
				yield return new Tuple<int, XmlNode>(indent, childNode);

				foreach (var node in AllNodesWithIndent(childNode, indent + 1))
				{
					yield return node;
				}
			}
		}
	}
}
