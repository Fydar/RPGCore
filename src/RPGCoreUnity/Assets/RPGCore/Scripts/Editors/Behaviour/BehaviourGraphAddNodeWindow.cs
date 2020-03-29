using RPGCore.Behaviour.Manifest;
using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class BehaviourGraphAddNodeDropdown : AdvancedDropdown
	{
		private readonly Action<object> onSelectNode;
		private readonly BehaviourManifest manifest;

		public BehaviourGraphAddNodeDropdown(BehaviourManifest manifest, Action<object> onSelectNode)
			: base(new AdvancedDropdownState())
		{
			minimumSize = new Vector2(270, 308);
			this.manifest = manifest;
			this.onSelectNode = onSelectNode;
		}

		protected override AdvancedDropdownItem BuildRoot()
		{
			var root = new AdvancedDropdownItem("Nodes");

			foreach (var node in manifest.Types.NodeTypes)
			{
				var item = new AddNodeDropdownItem(node.Key);
				root.AddChild(item);
			}

			return root;
		}

		protected override void ItemSelected(AdvancedDropdownItem item)
		{
			onSelectNode(((AddNodeDropdownItem)item).Node);
		}

		private class AddNodeDropdownItem : AdvancedDropdownItem
		{
			public string Node { get; }

			public AddNodeDropdownItem(string node)
				: base(node)
			{
				Node = node;
			}
		}
	}
}
