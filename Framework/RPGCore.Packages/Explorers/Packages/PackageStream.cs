using System;
using System.IO;

namespace RPGCore.Packages
{
    public class PackageStream : Stream, IDisposable
    {
        public Stream InternalStream;
        public IDisposable[] Components;

		private bool Disposed;

        public override bool CanRead => InternalStream.CanRead;

        public override bool CanSeek => InternalStream.CanSeek;

        public override bool CanWrite => InternalStream.CanWrite;

        public override long Length => InternalStream.Length;

        public override long Position { get => InternalStream.Position; set => InternalStream.Position = value; }

        public PackageStream(Stream internalStream, params IDisposable[] components)
        {
            InternalStream = internalStream;
            Components = components;
        }

        public override void Flush()
        {
            InternalStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return InternalStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return InternalStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            InternalStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            InternalStream.Write(buffer, offset, count);
        }

		~PackageStream()
		{
			Dispose(false);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (!Disposed)
			{
				foreach (var component in Components)
				{
					component.Dispose();
				}
				Disposed = true;
			}
		}
    }
}
