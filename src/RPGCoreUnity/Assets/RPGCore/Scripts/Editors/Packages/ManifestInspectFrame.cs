﻿using RPGCore.DataEditor.Manifest;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RPGCoreUnity.Editors
{
	public class ManifestInspectFrame : WindowFrame
	{
		public BehaviourManifest Manifest { get; private set; }

		[SerializeField] private TreeViewState manifestTreeViewState;
		private BehaviourManifestTreeView manifestTreeView;

		public ManifestInspectFrame(BehaviourManifest manifest)
		{
			Manifest = manifest;
		}

		public override void OnEnable()
		{
		}

		public override void OnGUI()
		{
			if (manifestTreeView == null)
			{
				if (manifestTreeViewState == null)
				{
					manifestTreeViewState = new TreeViewState();
				}
				manifestTreeView = new BehaviourManifestTreeView(manifestTreeViewState);
			}
			Debug.Log(Manifest.ToString());

			manifestTreeView.SetTarget(Manifest);

			manifestTreeView.OnGUI(Position);
		}
	}
}
