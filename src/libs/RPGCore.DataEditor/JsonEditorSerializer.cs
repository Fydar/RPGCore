using RPGCore.DataEditor.Manifest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace RPGCore.DataEditor
{
	/// <inheritdoc/>
	public class JsonEditorSerializer : IEditorSerializer
	{
		/// <inheritdoc/>
		public IEditorValue DeserializeValue(EditorSession session, SchemaQualifiedType type, ReadOnlySpan<byte> output)
		{
			var reader = new Utf8JsonReader(output, true, new JsonReaderState(new JsonReaderOptions()
			{
				CommentHandling = JsonCommentHandling.Allow,
				AllowTrailingCommas = true,
			}));

			if (!reader.Read())
			{
				throw new InvalidOperationException("Unable to read value as the data is empty.");
			}

			return ReadValue(session, type, ref reader);
		}

		/// <inheritdoc/>
		public void SerializeValue(IEditorValue value, Stream output)
		{
			using var writer = new Utf8JsonWriter(output, new JsonWriterOptions()
			{
				Indented = true
			});
			SerializeValue(value, writer);
		}

		private void SerializeValue(IEditorValue value, Utf8JsonWriter writer)
		{
			foreach (string comment in value.Comments)
			{
				writer.WriteCommentValue(comment);
			}

			switch (value)
			{
				case EditorScalarValue editorScalar:
				{
					switch (editorScalar.Value)
					{
						case byte[] byteArray:
						{
							writer.WriteBase64StringValue(byteArray);
							break;
						}
						case Memory<byte> byteArray:
						{
							writer.WriteBase64StringValue(byteArray.Span);
							break;
						}
						case ArraySegment<byte> byteArray:
						{
							writer.WriteBase64StringValue(byteArray.AsSpan());
							break;
						}
						case string stringValue:
						{
							writer.WriteStringValue(stringValue);
							break;
						}
						case char characterValue:
						{
							writer.WriteStringValue(characterValue.ToString());
							break;
						}
						case bool booleanValue:
						{
							writer.WriteBooleanValue(booleanValue);
							break;
						}
						case byte numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case sbyte numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case short numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case ushort numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case int numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case uint numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case long numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case ulong numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case decimal numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case double numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case float numberValue:
						{
							writer.WriteNumberValue(numberValue);
							break;
						}
						case null:
						{
							writer.WriteNullValue();
							break;
						}
						default:
						{
							throw new InvalidOperationException($"unknown type {editorScalar?.Value?.GetType().Name ?? "null"}");
						}
					}
					break;
				}
				case EditorObject editorObject:
				{
					writer.WriteStartObject();

					foreach (var field in editorObject.Fields)
					{
						foreach (string comment in field.Comments)
						{
							writer.WriteCommentValue(comment);
						}

						writer.WritePropertyName(field.Name);
						SerializeValue(field.Value, writer);
					}

					writer.WriteEndObject();
					break;
				}
				case EditorDictionary editorDictionary:
				{
					writer.WriteStartObject();

					foreach (var kvp in editorDictionary.KeyValuePairs)
					{
						foreach (string comment in kvp.Comments)
						{
							writer.WriteCommentValue(comment);
						}

						var editorScalar = kvp.Key as EditorScalarValue;

						string? key = editorScalar?.Value as string;
						writer.WritePropertyName(key!);
						SerializeValue(kvp.Value, writer);
					}

					writer.WriteEndObject();
					break;
				}
				case EditorList editorList:
				{
					writer.WriteStartArray();

					foreach (var element in editorList.Elements)
					{
						foreach (string comment in element.Comments)
						{
							writer.WriteCommentValue(comment);
						}

						SerializeValue(element, writer);
					}

					writer.WriteEndArray();
					break;
				}
				case EditorNull:
				{
					writer.WriteNullValue();
					break;
				}
				case null:
					throw new ArgumentNullException("Cannot serialize \"null\" editor value.");
				default:
					throw new ArgumentException($"Cannot serialize editor value of type \"{value.GetType().Name}\".");
			}
		}

		private IEditorValue ReadValue(EditorSession session, SchemaQualifiedType type, ref Utf8JsonReader reader)
		{
			if (type.Identifier == "[Array]")
			{
				return ReadArray(session, type, ref reader);
			}
			else if (type.Identifier == "[Dictionary]")
			{
				return ReadDictionary(session, type, ref reader);
			}
			else
			{
				var typeInfo = session.ResolveType(type);

				if (typeInfo == null)
				{
					throw new InvalidOperationException($"Unable to resolve type information for \"{type}\".");
				}

				if (typeInfo.Fields != null && typeInfo.Fields.Count > 0)
				{
					return ReadObject(session, type, ref reader);
				}
				else
				{
					return ReadScalarValue(session, type, ref reader);
				}
			}
		}

		private IEditorValue ReadObject(EditorSession session, SchemaQualifiedType type, ref Utf8JsonReader reader)
		{
			var comments = ReadComments(ref reader);

			var tokenType = reader.TokenType;
			if (tokenType == JsonTokenType.Null)
			{
				return new EditorNull(session);
			}
			else if (tokenType != JsonTokenType.StartObject)
			{
				throw new InvalidDataException("Unexpected token.");
			}

			var editorObject = new EditorObject(session, type);

			foreach (string comment in comments)
			{
				editorObject.Comments.Add(comment);
			}

			while (reader.Read())
			{
				var fieldComments = ReadComments(ref reader);

				switch (reader.TokenType)
				{
					case JsonTokenType.EndObject:
						return editorObject;

					case JsonTokenType.PropertyName:
						string? propertyName = reader.GetString();
						var field = editorObject.Fields.FirstOrDefault(f => f.Name == propertyName);

						if (!reader.Read())
						{
							throw new InvalidOperationException("Unable to read value as the data is empty.");
						}

						var fieldValue = ReadValue(session, field.Schema.Type, ref reader);
						field.Value = fieldValue;

						foreach (string comment in fieldComments)
						{
							field.Comments.Add(comment);
						}
						break;

					case JsonTokenType.Comment:

						break;

					default:
						throw new ArgumentException();
				}
			}

			return new EditorNull(session);
		}

		private IEditorValue ReadArray(EditorSession session, SchemaQualifiedType type, ref Utf8JsonReader reader)
		{
			var comments = ReadComments(ref reader);

			var tokenType = reader.TokenType;
			if (tokenType == JsonTokenType.Null)
			{
				return new EditorNull(session);
			}
			else if (tokenType != JsonTokenType.StartArray)
			{
				throw new InvalidDataException($"Unexpected token {tokenType}.");
			}

			var elementType = type.TemplateTypes[0];
			var editorList = new EditorList(session, type);

			foreach (string comment in comments)
			{
				editorList.Comments.Add(comment);
			}

			while (reader.Read())
			{
				var elementComments = ReadComments(ref reader);

				switch (reader.TokenType)
				{
					case JsonTokenType.EndArray:
						return editorList;

					case JsonTokenType.Comment:
						break;

					default:
						var elementValue = ReadValue(session, elementType, ref reader);

						foreach (string comment in elementComments)
						{
							elementValue.Comments.Add(comment);
						}

						editorList.Elements.Add(elementValue);
						break;
				}
			}

			throw new InvalidOperationException("Invalid syntax");
		}

		private IEditorValue ReadDictionary(EditorSession session, SchemaQualifiedType type, ref Utf8JsonReader reader)
		{
			var comments = ReadComments(ref reader);

			var tokenType = reader.TokenType;
			if (tokenType == JsonTokenType.Null)
			{
				return new EditorNull(session);
			}
			else if (tokenType != JsonTokenType.StartObject)
			{
				throw new InvalidDataException($"Unexpected token {tokenType}.");
			}

			var keyType = type.TemplateTypes[0];
			var valueType = type.TemplateTypes[1];
			var editorDictionary = new EditorDictionary(session, keyType, valueType);

			foreach (string comment in comments)
			{
				editorDictionary.Comments.Add(comment);
			}

			while (reader.Read())
			{
				var keyValuePairComments = ReadComments(ref reader);

				switch (reader.TokenType)
				{
					case JsonTokenType.EndObject:
						return editorDictionary;

					case JsonTokenType.Comment:
						break;

					case JsonTokenType.PropertyName:
						var keyValue = ReadScalarValue(session, keyType, ref reader);

						if (!reader.Read())
						{
							throw new InvalidOperationException("Unable to read value as the data is empty.");
						}

						var elementValue = ReadValue(session, valueType, ref reader);

						var kvp = new EditorKeyValuePair(editorDictionary, keyValue, elementValue);

						foreach (string comment in keyValuePairComments)
						{
							kvp.Comments.Add(comment);
						}

						editorDictionary.KeyValuePairs.Add(kvp);
						break;

					default:
						throw new InvalidOperationException("Invalid syntax");
				}
			}

			throw new InvalidOperationException("Invalid syntax");
		}

		private IEditorValue ReadScalarValue(EditorSession session, SchemaQualifiedType type, ref Utf8JsonReader reader)
		{
			var comments = ReadComments(ref reader);

			if (reader.TokenType == JsonTokenType.Null)
			{
				if (type.IsNullable)
				{
					return new EditorNull(session);
				}
				else
				{
					throw new InvalidOperationException($"Unable to assign null value to type {type}.");
				}
			}

			var editorScalarValue = type.Identifier switch
			{
				"string" => new EditorScalarValue(session, type, reader.GetString()),
				"char" => new EditorScalarValue(session, type, reader.GetString()![0]),
				"bool" => new EditorScalarValue(session, type, reader.GetBoolean()),
				"byte" => new EditorScalarValue(session, type, reader.GetByte()),
				"sbyte" => new EditorScalarValue(session, type, reader.GetSByte()),
				"short" => new EditorScalarValue(session, type, reader.GetInt16()),
				"ushort" => new EditorScalarValue(session, type, reader.GetUInt16()),
				"int" => new EditorScalarValue(session, type, reader.GetInt32()),
				"uint" => new EditorScalarValue(session, type, reader.GetUInt32()),
				"long" => new EditorScalarValue(session, type, reader.GetInt64()),
				"ulong" => new EditorScalarValue(session, type, reader.GetUInt64()),
				"float" => new EditorScalarValue(session, type, reader.GetSingle()),
				"double" => new EditorScalarValue(session, type, reader.GetDouble()),
				"decimal" => new EditorScalarValue(session, type, reader.GetDecimal()),
				_ => throw new InvalidOperationException($"unknown type {type.Identifier}"),
			};

			foreach (string comment in comments)
			{
				editorScalarValue.Comments.Add(comment);
			}

			return editorScalarValue;
		}
		private IList<string> ReadComments(ref Utf8JsonReader reader)
		{
			if (reader.TokenType != JsonTokenType.Comment)
			{
				return Array.Empty<string>();
			}

			var comments = new List<string>();
			while (reader.TokenType == JsonTokenType.Comment)
			{
				comments.Add(reader.GetComment());
				reader.Read();
			}
			return comments;
		}
	}
}
