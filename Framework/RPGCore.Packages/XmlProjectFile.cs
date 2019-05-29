using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace RPGCore.Packages
{
    public class XmlProjectFile
    {
        protected XmlDocument Document;
        public string Path;

        public XmlProjectFile(XmlDocument document)
        {
            Document = document;
        }

        public static XmlProjectFile Load(string path)
        {
            if (!File.Exists(path))
                return null;

            var doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(path);

            var model = new XmlProjectFile(doc);
            model.Path = path;
            return model;
        }

        public void Format()
        {
            Minify();

            var nodes = AllNodesWithIndent(Document.DocumentElement, 1).ToArray();

            Document.DocumentElement.InsertBefore(Document.CreateWhitespace("\n\n"), Document.DocumentElement.FirstChild);

            foreach (var indentedNode in nodes)
            {
                var childNode = indentedNode.Item2;

                if (childNode.NodeType == XmlNodeType.Element
                    || childNode.NodeType == XmlNodeType.Comment)
                {
                    int indent = indentedNode.Item1;

                    // Indent opening tag
                    var startIndent = Document.CreateWhitespace(new string(' ', indent * 2));
                    childNode.ParentNode.InsertBefore(startIndent, childNode);

                    if (HasChildElement(childNode))
                    {
                        childNode.ParentNode.InsertAfter(Document.CreateWhitespace("\n\n"), childNode);

                        childNode.InsertBefore(Document.CreateWhitespace("\n"), childNode.FirstChild);
                        
                        if (HasChildObjects(childNode))
                        {
                            childNode.InsertBefore(Document.CreateWhitespace("\n"), childNode.FirstChild);
                        }

                        // Indent closing tag
                        var endIndent = Document.CreateWhitespace(new string(' ', indent * 2));
                        childNode.InsertAfter(endIndent, childNode.LastChild);
                    }
                    else
                    {
                        childNode.ParentNode.InsertAfter(Document.CreateWhitespace("\n"), childNode);
                    }
                }
            }
        }

        private static bool HasChildElement(XmlNode node)
        {
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                var childNode = node.ChildNodes[i];

                if (childNode.NodeType == XmlNodeType.Element)
                    return true;
            }
            return false;
        }

        private static bool HasChildObjects(XmlNode node)
        {
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                var childNode = node.ChildNodes[i];

                if (HasChildElement(childNode))
                    return true;
            }
            return false;
        }

        public void Minify()
        {
            var allNodes = AllNodes(Document.DocumentElement).ToArray();
            foreach (var node in allNodes)
            {
                if (node.NodeType == XmlNodeType.SignificantWhitespace
                || node.NodeType == XmlNodeType.Whitespace)
                {
                    node.ParentNode.RemoveChild(node);
                }
            }
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

        public void Save(string path)
        {
            Document.Save(path);
        }
    }
}