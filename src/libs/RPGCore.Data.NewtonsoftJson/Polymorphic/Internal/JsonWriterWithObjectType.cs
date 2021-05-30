using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RPGCore.Data.NewtonsoftJson.Polymorphic.Internal
{
	internal class JsonWriterWithObjectType : JsonWriter
	{
		private readonly JsonWriter innerWriter;
		private readonly string fieldName;
		private readonly string discriminator;
		private bool hasWroteDiscriminator;

		public JsonWriterWithObjectType(string fieldName, string discriminator, JsonWriter writer)
		{
			hasWroteDiscriminator = false;
			this.fieldName = fieldName;
			this.discriminator = discriminator;
			innerWriter = writer;
		}

		public override void WriteStartObject()
		{
			innerWriter.WriteStartObject();

			if (hasWroteDiscriminator == false)
			{
				innerWriter.WritePropertyName(fieldName);
				innerWriter.WriteValue(discriminator);
				hasWroteDiscriminator = true;
			}
		}

		public void Dispose()
		{
			((IDisposable)innerWriter).Dispose();
		}

		public override Task CloseAsync(CancellationToken cancellationToken = default)
		{
			return innerWriter.CloseAsync(cancellationToken);
		}

		public override Task FlushAsync(CancellationToken cancellationToken = default)
		{
			return innerWriter.FlushAsync(cancellationToken);
		}

		public override Task WriteRawAsync(string? json, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteRawAsync(json, cancellationToken);
		}

		public override Task WriteEndAsync(CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteEndAsync(cancellationToken);
		}

		public override Task WriteEndArrayAsync(CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteEndArrayAsync(cancellationToken);
		}

		public override Task WriteEndConstructorAsync(CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteEndConstructorAsync(cancellationToken);
		}

		public override Task WriteEndObjectAsync(CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteEndObjectAsync(cancellationToken);
		}

		public override Task WriteNullAsync(CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteNullAsync(cancellationToken);
		}

		public override Task WritePropertyNameAsync(string name, CancellationToken cancellationToken = default)
		{
			return innerWriter.WritePropertyNameAsync(name, cancellationToken);
		}

		public override Task WritePropertyNameAsync(string name, bool escape, CancellationToken cancellationToken = default)
		{
			return innerWriter.WritePropertyNameAsync(name, escape, cancellationToken);
		}

		public override Task WriteStartArrayAsync(CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteStartArrayAsync(cancellationToken);
		}

		public override Task WriteCommentAsync(string? text, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteCommentAsync(text, cancellationToken);
		}

		public override Task WriteRawValueAsync(string? json, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteRawValueAsync(json, cancellationToken);
		}

		public override Task WriteStartConstructorAsync(string name, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteStartConstructorAsync(name, cancellationToken);
		}

		public override Task WriteStartObjectAsync(CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteStartObjectAsync(cancellationToken);
		}

		public override Task WriteValueAsync(bool value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(bool? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(byte value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(byte? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(byte[]? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(char value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(char? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(DateTime value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(DateTime? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(DateTimeOffset value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(DateTimeOffset? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(decimal value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(decimal? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(double value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(double? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(float value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(float? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(Guid value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(Guid? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(int value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(int? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(long value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(long? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(object? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(sbyte value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(sbyte? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(short value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(short? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(string? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(TimeSpan value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(TimeSpan? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(uint value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(uint? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(ulong value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(ulong? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(Uri? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(ushort value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteValueAsync(ushort? value, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteValueAsync(value, cancellationToken);
		}

		public override Task WriteUndefinedAsync(CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteUndefinedAsync(cancellationToken);
		}

		public override Task WriteWhitespaceAsync(string ws, CancellationToken cancellationToken = default)
		{
			return innerWriter.WriteWhitespaceAsync(ws, cancellationToken);
		}

		public override void Flush()
		{
			innerWriter.Flush();
		}

		public override void Close()
		{
			innerWriter.Close();
		}

		public override void WriteEndObject()
		{
			innerWriter.WriteEndObject();
		}

		public override void WriteStartArray()
		{
			innerWriter.WriteStartArray();
		}

		public override void WriteEndArray()
		{
			innerWriter.WriteEndArray();
		}

		public override void WriteStartConstructor(string name)
		{
			innerWriter.WriteStartConstructor(name);
		}

		public override void WriteEndConstructor()
		{
			innerWriter.WriteEndConstructor();
		}

		public override void WritePropertyName(string name)
		{
			innerWriter.WritePropertyName(name);
		}

		public override void WritePropertyName(string name, bool escape)
		{
			innerWriter.WritePropertyName(name, escape);
		}

		public override void WriteEnd()
		{
			innerWriter.WriteEnd();
		}

		public override void WriteNull()
		{
			innerWriter.WriteNull();
		}

		public override void WriteUndefined()
		{
			innerWriter.WriteUndefined();
		}

		public override void WriteRaw(string? json)
		{
			innerWriter.WriteRaw(json);
		}

		public override void WriteRawValue(string? json)
		{
			innerWriter.WriteRawValue(json);
		}

		public override void WriteValue(string? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(int value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(uint value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(long value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(ulong value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(float value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(double value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(bool value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(short value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(ushort value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(char value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(byte value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(sbyte value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(decimal value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(DateTime value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(DateTimeOffset value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(Guid value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(TimeSpan value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(int? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(uint? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(long? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(ulong? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(float? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(double? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(bool? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(short? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(ushort? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(char? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(byte? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(sbyte? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(decimal? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(DateTime? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(DateTimeOffset? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(Guid? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(TimeSpan? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(byte[]? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(Uri? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteValue(object? value)
		{
			innerWriter.WriteValue(value);
		}

		public override void WriteComment(string? text)
		{
			innerWriter.WriteComment(text);
		}

		public override void WriteWhitespace(string ws)
		{
			innerWriter.WriteWhitespace(ws);
		}
	}
}
