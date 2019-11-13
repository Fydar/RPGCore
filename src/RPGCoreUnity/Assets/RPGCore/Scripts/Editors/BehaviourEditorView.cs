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
		[SerializeField] private LocalPropertyId connectionOutput;
		[SerializeField] private bool isOutputSocket;

		public string DescribeCurrentAction
		{
			get
			{
				switch (currentMode)
				{
					case Mode.NodeDragging:

						if (Selection.Count == 1)
						{
							return $"Dragging nodes {string.Join (", ", Selection)}";
						}
						else
						{
							return $"Dragging node {string.Join (", ", Selection)}";
						}

					case Mode.ViewDragging:
						return $"Panning to {panPosition}";

					case Mode.CreatingConnection:
						if (IsOutputSocket)
						{
							return $"Creating connection from {connectionOutput}";
						}
						else
						{
							return $"Creating connection to {connectionOutput}";
						}

					default:
						return "None";
				}
			}
		}

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

		public LocalPropertyId ConnectionOutput => connectionOutput;

		public EditorField ConnectionInput { get; set; }
		public string ConnectionInputNodeId { get; set; }

		public bool IsOutputSocket => isOutputSocket;

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

		public void BeginSession(EditorSession session)
		{
			Session = session;
		}

		public void BeginConnectionFromOutput(LocalPropertyId connectionStart)
		{
			currentMode = Mode.CreatingConnection;
			connectionOutput = connectionStart;
			isOutputSocket = true;
		}

		public void BeginConnectionFromInput(EditorField connectionEnd, string connectionInputNodeId)
		{
			ConnectionInput = connectionEnd;
			ConnectionInputNodeId = connectionInputNodeId;

			currentMode = Mode.CreatingConnection;
			isOutputSocket = false;
		}
	}
}
