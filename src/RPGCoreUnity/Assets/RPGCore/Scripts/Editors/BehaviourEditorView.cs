using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	[Serializable]
	public class BehaviourEditorView
	{
		public enum Mode
		{
			None,
			NodeDragging,
			ViewDragging,
			CreatingConnection
		}

		[SerializeField] private Mode currentMode;
		[SerializeField] private Vector2 panPosition;
		[SerializeField] private HashSet<string> selection;
		[SerializeField] private LocalPropertyId connectionStart;

		public HashSet<string> Selection
		{
			get
			{
				if (selection == null)
				{
					selection = new HashSet<string> ();
				}
				return selection;
			}
		}

		public Vector2 PanPosition
		{
			get
			{
				return panPosition;
			}
			set
			{
				panPosition = value;
			}
		}

		public LocalPropertyId ConnectionStart
		{
			get
			{
				return connectionStart;
			}
			set
			{
				connectionStart = value;
			}
		}

		public Mode CurrentMode
		{
			get
			{
				return currentMode;
			}
			set
			{
				currentMode = value;
			}
		}

		public EditorSession Session { get; private set; }

		public void BeginSession (EditorSession session)
		{
			Session = session;
		}
	}
}
