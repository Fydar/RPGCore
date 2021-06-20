using System.ComponentModel;

namespace RPGCore.DataEditor
{
	public class EditorUIFeature : IEditorFeature, INotifyPropertyChanged
	{
		private bool isExpanded;

		public bool IsExpanded
		{
			get
			{
				return isExpanded;
			}
			set
			{
				if (IsExpanded != value)
				{
					isExpanded = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(IsExpanded)));
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public EditorUIFeature()
		{
			PropertyChanged = delegate { };
		}

		public void AttachToToken(IEditorToken token)
		{
		}

		public void RemoveFromToken(IEditorToken token)
		{
		}
	}
}
