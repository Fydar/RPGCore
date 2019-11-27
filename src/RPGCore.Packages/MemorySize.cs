using System;
using System.IO;
using System.Text;

namespace RPGCore.Packages
{
	/// <summary>
	/// A simple structure that formats for bytes. 
	/// </summary>
	public struct MemorySize : IEquatable<MemorySize>, IComparable<MemorySize>, IFormattable
	{
		public const ulong Denomination = 1000;

		private const ulong kilobyteSize = Denomination;
		private const ulong megabyteSize = kilobyteSize * Denomination;
		private const ulong gigabyteSize = megabyteSize * Denomination;
		private const ulong terrabyteSize = gigabyteSize * Denomination;

		private const string byteSuffix = "b";
		private const string kilobyteSuffix = "kB";
		private const string megabyteSuffix = "MB";
		private const string gigabyteSuffix = "GB";

		public ulong Bytes { get; set; }

		public double Kilobytes
		{
			get
			{
				return ((double)Bytes) / kilobyteSize;
			}
			set
			{
				Bytes = (ulong)(value * kilobyteSize);
			}
		}

		public double Megabytes
		{
			get
			{
				return ((double)Bytes) / megabyteSize;
			}
			set
			{
				Bytes = (ulong)(value * megabyteSize);
			}
		}

		public double Gigabytes
		{
			get
			{
				return ((double)Bytes) / gigabyteSize;
			}
			set
			{
				Bytes = (ulong)(value * gigabyteSize);
			}
		}

		public double Terrabytes
		{
			get
			{
				return ((double)Bytes) / terrabyteSize;
			}
			set
			{
				Bytes = (ulong)(value * terrabyteSize);
			}
		}

		public MemorySize(long bytes)
		{
			Bytes = (ulong)bytes;
		}

		public MemorySize(ulong bytes)
		{
			Bytes = bytes;
		}

		public static MemorySize SizeOf(string data)
		{
			return new MemorySize ((ulong)data.Length * 2);
		}

		public static MemorySize SizeOf(string data, Encoding encoding)
		{
			return new MemorySize ((ulong)encoding.GetByteCount (data));
		}

		public static MemorySize SizeOf(byte[] data)
		{
			return new MemorySize ((ulong)data.Length);
		}

		public static MemorySize SizeOf(FileInfo file)
		{
			return new MemorySize ((ulong)file.Length);
		}

		public override string ToString()
		{
			if (Gigabytes < 1.0f)
			{
				if (Megabytes < 1.0f)
				{
					if (Kilobytes < 1.0f)
					{
						return $"{Bytes.ToString ()} {byteSuffix}";
					}
					return $"{Kilobytes.ToString ()} {kilobyteSuffix}";
				}
				return $"{Megabytes.ToString ()} {megabyteSuffix}";
			}
			return $"{Gigabytes.ToString ()} {gigabyteSuffix}";
		}

		public override bool Equals(object obj) => obj is MemorySize size && Equals (size);

		public bool Equals(MemorySize other) => Bytes == other.Bytes;

		public override int GetHashCode() => 1182642244 + Bytes.GetHashCode ();

		public int CompareTo(MemorySize other) => Bytes.CompareTo (other.Bytes);

		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (Gigabytes < 1.0f)
			{
				if (Megabytes < 1.0f)
				{
					if (Kilobytes < 1.0f)
					{
						return $"{Bytes.ToString (format, formatProvider)} {byteSuffix}";
					}
					return $"{Kilobytes.ToString (format, formatProvider)} {kilobyteSuffix}";
				}
				return $"{Megabytes.ToString (format, formatProvider)} {megabyteSuffix}";
			}
			return $"{Gigabytes.ToString (format, formatProvider)} {gigabyteSuffix}";
		}

		public static bool operator ==(MemorySize left, MemorySize right)
		{
			return left.Equals (right);
		}

		public static bool operator !=(MemorySize left, MemorySize right)
		{
			return !(left == right);
		}

		public static MemorySize operator +(MemorySize left, MemorySize right)
		{
			return new MemorySize (left.Bytes + right.Bytes);
		}

		public static MemorySize operator -(MemorySize left, MemorySize right)
		{
			return new MemorySize (left.Bytes + right.Bytes);
		}

		public static MemorySize operator *(MemorySize left, double right)
		{
			return new MemorySize (Convert.ToUInt64 (left.Bytes * right));
		}

		public static MemorySize operator /(MemorySize left, double right)
		{
			return new MemorySize (Convert.ToUInt64 (left.Bytes / right));
		}
	}
}
