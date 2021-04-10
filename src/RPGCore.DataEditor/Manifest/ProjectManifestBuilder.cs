using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RPGCore.DataEditor.Manifest
{
	/// <summary>
	/// A builder used for constructing <see cref="ProjectManifest"/>s.
	/// </summary>
	public sealed class ProjectManifestBuilder
	{
		private List<SchemaType> ObjectTypes { get; set; }
		private List<SchemaNode> NodeTypes { get; set; }
		private List<SchemaTypeConversion> TypeConversions { get; set; }

		internal ProjectManifestBuilder()
		{
			ObjectTypes = new List<SchemaType>();
			NodeTypes = new List<SchemaNode>();
			TypeConversions = new List<SchemaTypeConversion>();
		}

		/// <summary>
		/// Adds a <see cref="SchemaType"/> to the <see cref="ProjectManifest"/>.
		/// </summary>
		/// <param name="schemaType">The <see cref="SchemaType"/> to add to the <see cref="ProjectManifest"/>.</param>
		/// <returns>Returns the current instance of the <see cref="ProjectManifestBuilder"/>.</returns>
		public ProjectManifestBuilder AddType(SchemaType schemaType)
		{
			ObjectTypes.Add(schemaType);
			return this;
		}

		/// <summary>
		/// Adds a <see cref="SchemaNode"/> to the <see cref="ProjectManifest"/>.
		/// </summary>
		/// <param name="schemaNode">The <see cref="SchemaNode"/> to add to the <see cref="ProjectManifest"/>.</param>
		/// <returns>Returns the current instance of the <see cref="ProjectManifestBuilder"/>.</returns>
		public ProjectManifestBuilder AddNodeType(SchemaNode schemaNode)
		{
			NodeTypes.Add(schemaNode);
			return this;
		}

		/// <summary>
		/// Adds a <see cref="SchemaTypeConversion"/> to the <see cref="ProjectManifest"/>.
		/// </summary>
		/// <param name="typeConversion">The <see cref="SchemaTypeConversion"/> to add to the <see cref="ProjectManifest"/>.</param>
		/// <returns>Returns the current instance of the <see cref="ProjectManifestBuilder"/>.</returns>
		public ProjectManifestBuilder AddConversion(SchemaTypeConversion typeConversion)
		{
			TypeConversions.Add(typeConversion);
			return this;
		}

		/// <summary>
		/// Reads the <see cref="Stream"/> and adds to this <see cref="ProjectManifestBuilder"/>.
		/// </summary>
		/// <param name="data">The source <see cref="Stream"/> to read from.</param>
		/// <returns>Returns the current instance of the <see cref="ProjectManifestBuilder"/>.</returns>
		public ProjectManifestBuilder AddFrom(ReadOnlySpan<byte> data)
		{
			var source = Load(data);

			ObjectTypes.AddRange(source.ObjectTypes);
			NodeTypes.AddRange(source.NodeTypes);
			TypeConversions.AddRange(source.TypeConversions);
			return this;
		}

		/// <summary>
		/// Builds a new instance of the <see cref="ProjectManifest"/>.
		/// </summary>
		/// <returns>A new <see cref="ProjectManifest"/> build from the current state of this <see cref="ProjectManifestBuilder"/>.</returns>
		public ProjectManifest Build()
		{
			return new ProjectManifest(ObjectTypes, NodeTypes, TypeConversions);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="ProjectManifestBuilder"/>.
		/// </summary>
		/// <returns>A new <see cref="ProjectManifestBuilder"/>.</returns>
		public static ProjectManifestBuilder Create()
		{
			return new ProjectManifestBuilder();
		}

		/// <summary>
		/// Loads an instance of the <see cref="ProjectManifestBuilder"/>.
		/// </summary>
		/// <returns>An instance of <see cref="ProjectManifestBuilder"/> loaded from the <see cref="Stream"/>.</returns>
		public static ProjectManifestBuilder Load(ReadOnlySpan<byte> data)
		{
			var result = JsonSerializer.Deserialize<ProjectManifestBuilder>(data, new JsonSerializerOptions()
			{

			});

			return result ?? throw new InvalidDataException($"Failed to load {nameof(ProjectManifestBuilder)} from stream.");
		}
	}
}
