using System.IO;
using System.IO.Compression;

namespace RPGCore.Packages
{
	public sealed class PackageStream : Stream
	{
		private readonly FileStream fileStream;
		private readonly ZipArchive zipArchive;
		private readonly Stream internalStream;

		private bool disposed;

		public override bool CanRead => internalStream.CanRead;

		public override bool CanSeek => internalStream.CanSeek;

		public override bool CanWrite => internalStream.CanWrite;

		public override long Length => internalStream.Length;

		public override long Position { get => internalStream.Position; set => internalStream.Position = value; }

		public PackageStream(FileStream fileStream, ZipArchive zipArchive, Stream internalStream)
		{
			this.fileStream = fileStream;
			this.zipArchive = zipArchive;
			this.internalStream = internalStream;
		}

		public override void Flush()
		{
			internalStream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return internalStream.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return internalStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			internalStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			internalStream.Write(buffer, offset, count);
		}

		~PackageStream()
		{
			Dispose(false);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (!disposed)
			{
				internalStream.Dispose();
				zipArchive.Dispose();
				fileStream.Dispose();
				disposed = true;
			}
		}
	}
}
