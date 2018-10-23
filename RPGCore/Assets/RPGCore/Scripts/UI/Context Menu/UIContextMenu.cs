using UnityEngine;
using RPGCore.Inventories;
using RPGCore.Utility;

namespace RPGCore.UI
{
	public class UIContextMenu : MonoBehaviour
	{
		public static UIContextMenu Instance;

		public CanvasGroup Background;
		public FadeBool BackgroundFade = new FadeBool ();

		public CanvasMouse Mouse;
		public RectTransform ListView;

		[Space]

		public ContextButtonPool ButtonPool = new ContextButtonPool ();
		public RectTransformPool DividerPool = new RectTransformPool ();
		public ContextFolderPool FolderPool = new ContextFolderPool ();

		private void Awake ()
		{
			Close ();

			Instance = this;
		}

		private void Update ()
		{
			BackgroundFade.Update ();
			Background.alpha = BackgroundFade.Value;
			Background.blocksRaycasts = BackgroundFade.Target;

			if (Input.GetKeyDown (KeyCode.Escape))
			{
				Close ();
			}
		}

		public void Display (params IContextEntry[] entries)
		{
			ButtonPool.Flush ();
			DividerPool.Flush ();
			FolderPool.Flush ();

			ListView.position = Mouse.ScreenToCanvas (Input.mousePosition);

			for (int i = 0; i < entries.Length; i++)
			{
				if (entries[i] == null)
					continue;
				entries[i].Render (this, ListView);
			}

			BackgroundFade.Target = true;
			ListView.gameObject.SetActive (true);
		}

		public void Close ()
		{
			BackgroundFade.Target = false;
			ListView.gameObject.SetActive (false);

			ButtonPool.Flush ();
			DividerPool.Flush ();
			FolderPool.Flush ();
		}
	}
}

/*private void Start ()
{
	Display (
		new ContextEntry ("Trade", () => { Debug.Log ("HI"); }),
		new ContextEntry ("Follow", () => { Debug.Log ("HERRO"); }),
		new ContextDivider (),
		new ContextEntry ("View Character", () => { Debug.Log ("Heman"); }),

		new ContextFolderEntry ("Set Marker",
			new ContextEntry ("Marker 1", () => { Debug.Log ("HI"); }),
			new ContextEntry ("Marker 2", () => { Debug.Log ("HI"); }),
			new ContextEntry ("Marker 3", () => { Debug.Log ("HI"); }),
			new ContextEntry ("Marker 4", () => { Debug.Log ("HI"); }),
			new ContextEntry ("Marker 5", () => { Debug.Log ("HI"); }),
			new ContextEntry ("Marker 6", () => { Debug.Log ("HI"); })
		),

		new ContextFolderEntry ("Folder",
			new ContextEntry ("Child 1", () => { Debug.Log ("HI"); }),
			new ContextFolderEntry ("Folder",
				new ContextEntry ("Child 1", () => { Debug.Log ("HI"); }),
				new ContextEntry ("Child 2", () => { Debug.Log ("HERRO"); })
			)
		)
	);
}*/
