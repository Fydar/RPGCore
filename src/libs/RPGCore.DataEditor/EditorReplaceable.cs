using RPGCore.DataEditor.Manifest;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// An editable data structure that allows for the type contained within it to be changed.
	/// </summary>
	public class EditorReplaceable : IEditorToken
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<string> comments;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private TypeName type;

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorSession Session { get; }

		/// <summary>
		/// The value contained within this <see cref="EditorReplaceable"/>.
		/// </summary>
		public IEditorValue Value { get; set; }

		/// <summary>
		/// A collection of <see cref="IEditorFeature"/>s associated with this <see cref="EditorReplaceable"/>.
		/// </summary>
		public FeatureCollection<EditorReplaceable> Features { get; }

		/// <inheritdoc/>
		public IList<string> Comments => comments;

		/// <summary>
		/// The type that this value can be described with.
		/// </summary>
		public TypeName Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;

				// If our value is no longer valid for the new type.
				if (!Session.IsValueOfType(Value, type))
				{

				}
			}
		}

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		FeatureCollection IEditorToken.Features => Features;

		/// <inheritdoc/>
		public event PropertyChangedEventHandler PropertyChanged;

		internal EditorReplaceable(EditorSession session, TypeName type, IEditorValue value)
		{
			Session = session;
			Type = type;
			Value = value;
			comments = new List<string>();
			Features = new FeatureCollection<EditorReplaceable>(this);

			PropertyChanged = delegate { };
		}
	}
}
