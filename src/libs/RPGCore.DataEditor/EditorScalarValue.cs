using RPGCore.DataEditor.Manifest;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// An editable value.
	/// </summary>
	public class EditorScalarValue<T> : EditorScalarValue
	{
		/// <summary>
		/// A <typeparamref name="T"/> representing the value of this <see cref="EditorScalarValue"/>.
		/// </summary>
		public new T? Value
		{
			get => base.Value == null ? default : (T)base.Value;
			set => base.Value = value;
		}

		internal EditorScalarValue(EditorSession session, TypeName type) : base(session, type, default(T))
		{
		}

		internal EditorScalarValue(EditorSession session, TypeName type, T? value) : base(session, type, value)
		{
		}
	}

	/// <summary>
	/// An editable value.
	/// </summary>
	public class EditorScalarValue : IEditorValue
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object? internalValue;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<string> comments;

		/// <inheritdoc/>
		public IList<string> Comments => comments;

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorSession Session { get; }

		/// <summary>
		/// The type of this <see cref="EditorScalarValue"/>.
		/// </summary>
		public TypeName Type { get; }

		/// <summary>
		/// An <see cref="object"/> representing the value of this <see cref="EditorScalarValue"/>.
		/// </summary>
		public object? Value
		{
			get => internalValue;
			set
			{
				internalValue = value;

				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}
		}

		/// <summary>
		/// A collection of <see cref="IEditorFeature"/>s associated with this <see cref="EditorScalarValue"/>.
		/// </summary>
		public FeatureCollection<EditorScalarValue> Features { get; }

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		FeatureCollection IEditorToken.Features => Features;

		/// <inheritdoc/>
		public event PropertyChangedEventHandler PropertyChanged;

		internal EditorScalarValue(EditorSession session, TypeName type)
		{
			Session = session;
			comments = new List<string>();
			Features = new FeatureCollection<EditorScalarValue>(this);

			Type = type;

			PropertyChanged = delegate { };
		}

		internal EditorScalarValue(EditorSession session, TypeName type, object? value)
			: this(session, type)
		{
			Value = value;
		}
	}
}
