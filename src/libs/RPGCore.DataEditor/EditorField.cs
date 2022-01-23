using RPGCore.DataEditor.Manifest;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace RPGCore.DataEditor;

/// <summary>
/// An editable hard-typed container for a value.
/// </summary>
public class EditorField : IEditorToken
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly List<string> comments;

	/// <inheritdoc/>
	public IList<string> Comments => comments;

	/// <inheritdoc/>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public EditorSession Session => Parent.Session;

	/// <summary>
	/// The <see cref="EditorObject"/> that this <see cref="EditorField"/> belongs to.
	/// </summary>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public EditorObject Parent { get; }

	/// <summary>
	/// The name of this field.
	/// </summary>
	public string Name { get; } = string.Empty;

	/// <summary>
	/// The value contained within this <see cref="EditorField"/>.
	/// </summary>
	public EditorReplaceable Value { get; }

	/// <summary>
	/// A collection of <see cref="IEditorFeature"/>s associated with this <see cref="EditorField"/>.
	/// </summary>
	public FeatureCollection<EditorField> Features { get; }

	/// <summary>
	/// A <see cref="SchemaField"/> used to drive the behaviour of this <see cref="EditorField"/>.
	/// </summary>
	public SchemaField? Schema
	{
		get
		{
			var parentType = Session.ResolveType(Parent.Type);

			if (parentType?.Fields == null)
			{
				return null;
			}

			foreach (var field in parentType.Fields)
			{
				if (field.Name == Name)
				{
					return field;
				}
			}
			return null;
		}
	}

	/// <inheritdoc/>
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	FeatureCollection IEditorToken.Features => Features;

	/// <inheritdoc/>
	public event PropertyChangedEventHandler PropertyChanged;

	internal EditorField(EditorObject parent, string name)
	{
		Parent = parent;
		Name = name;

		Value = new EditorReplaceable(parent.Session, Schema.Type, parent.Session.CreateDefaultValue(Schema.Type));
		Features = new FeatureCollection<EditorField>(this);
		comments = new List<string>();

		PropertyChanged = delegate { };
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		if (Schema == null)
		{
			return $"unknown {Name}";
		}
		return Value.Value switch
		{
			EditorNull => $"{Schema.Type} {Schema.Name} = null",
			EditorScalarValue editorScalarValue => $"{Schema.Type} {Schema.Name} = {editorScalarValue.Value}",
			EditorDictionary => $"{Schema.Type} {Schema.Name} = {{ ... }}",
			EditorList editorList => $"{Schema.Type} {Schema.Name} = [{editorList.Elements.Count}]",
			_ => $"{Schema.Type} {Schema.Name}",
		};
	}
}
