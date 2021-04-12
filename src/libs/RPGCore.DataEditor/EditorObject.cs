using RPGCore.DataEditor.Manifest;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// An editable data structure that uses hard-typed fields.
	/// </summary>
	public class EditorObject : IEditorValue
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly List<string> comments;

		/// <inheritdoc/>
		public IList<string> Comments => comments;

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public EditorSession Session { get; }

		/// <summary>
		/// The type of the current instance of the <see cref="EditorObject"/>.
		/// </summary>
		public SchemaQualifiedType Type { get; }

		/// <summary>
		/// All <see cref="EditorField"/> contained within this object.
		/// </summary>
		public List<EditorField> Fields { get; }

		internal EditorObject(EditorSession session, SchemaQualifiedType type)
		{
			Session = session;
			Type = type;

			Fields = new List<EditorField>();

			comments = new List<string>();

			AddMissingFields();
		}

		private void AddMissingFields()
		{
			bool ContainsField(string field)
			{
				foreach (var otherField in Fields)
				{
					if (otherField.Name == field)
					{
						return true;
					}
				}
				return false;
			}

			var type = Session.Manifest.GetTypeInformation(Type.Identifier);

			if (type.Fields == null)
			{
				throw new InvalidOperationException($"Cannot use \"{Type}\" for an object as it doesn't have any fields.");
			}
			foreach (var field in type.Fields)
			{
				if (!ContainsField(field.Name))
				{
					Fields.Add(new EditorField(this, field.Name)
					{
						Value = Session.CreateDefaultValue(field.Type)
					});
				}
			}
		}
	}
}
