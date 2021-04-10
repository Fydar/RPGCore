using RPGCore.DataEditor.Manifest;
using System;
using System.Text;

namespace RPGCore.DataEditor
{
	/// <summary>
	/// Provides configuration for data editing.
	/// </summary>
	public class EditorSession
	{
		/// <summary>
		/// The manifest used for configuring the data editor.
		/// </summary>
		public ProjectManifest Manifest { get; }

		/// <summary>
		/// The serializer used to load and save values.
		/// </summary>
		public IEditorSerializer Serializer { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="EditorSession"/> object.
		/// </summary>
		/// <param name="manifest">A manifest used to drive data behaviour.</param>
		/// <param name="serializer">The serializer used to serialize values.</param>
		public EditorSession(ProjectManifest manifest, IEditorSerializer serializer)
		{
			Manifest = manifest;
			Serializer = serializer;
		}

		/// <summary>
		/// Starts editing a file using configuration provided by this <see cref="EditorSession"/>.
		/// </summary>
		/// <returns>A new <see cref="EditorFile"/> used for editing.</returns>
		public EditorFile EditFile(SchemaQualifiedType schema)
		{
			return new EditorFile(this, schema);
		}

		/// <summary>
		/// Gets the underlying type information from a qualified type.
		/// </summary>
		/// <param name="schemaQualifiedType"></param>
		/// <returns></returns>
		public SchemaType? ResolveType(SchemaQualifiedType schemaQualifiedType)
		{
			var type = Manifest.GetTypeInformation(schemaQualifiedType.Identifier);

			return type;
		}

		internal IEditorValue CreateDefaultValue(SchemaQualifiedType qualifiedType)
		{
			return qualifiedType.IsNullable
				? new EditorNull(this)
				: CreateInstatedValue(qualifiedType);
		}

		internal IEditorValue CreateInstatedValue(SchemaQualifiedType qualifiedType)
		{
			if (qualifiedType.Identifier == "[Dictionary]")
			{
				return new EditorDictionary(this, qualifiedType.TemplateTypes[0], qualifiedType.TemplateTypes[1]);
			}
			else if (qualifiedType.Identifier == "[Array]")
			{
				return new EditorList(this, qualifiedType.TemplateTypes[0]);
			}
			else
			{
				var typeInfo = Manifest.GetTypeInformation(qualifiedType.Identifier);
				if (typeInfo == null)
				{
					throw new InvalidOperationException($"Cannot create an instance of an object of type \"{typeInfo}\".");
				}

				if (string.IsNullOrEmpty(typeInfo.InstatedValue))
				{
					return new EditorNull(this);
				}
				else
				{
					byte[]? data = Encoding.UTF8.GetBytes(typeInfo.InstatedValue);
					var scalarInnerValue = Serializer.DeserializeValue(this, qualifiedType, data);
					return scalarInnerValue;
				}
			}
		}
	}
}
