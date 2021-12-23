using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RPGCoreUnity.Editors
{
	public abstract class WindowFrame
	{
		public Rect Position;
		public EditorWindow Window;

		public bool HasUnsavedChanges { get; protected set; }

		public abstract void OnEnable();
		public abstract void OnGUI();
		public virtual IEnumerable<FrameTab> SpawnChildren()
		{
			return Enumerable.Empty<FrameTab>();
		}
	}
}
