using UnityEngine;

namespace RPGCore.Utility
{
	[CreateAssetMenu (menuName = "RPGCore/Utility/DrawerLibrary Resources")]
	public class DrawerLibraryResources : ResourceCollection<DrawerLibraryResources>
	{
		[ErrorIfNull]
		public Texture2D CheckIcon;
		[ErrorIfNull]
		public Texture2D WarningIcon;
		[ErrorIfNull]
		public Texture2D ErrorIcon;
	}
}