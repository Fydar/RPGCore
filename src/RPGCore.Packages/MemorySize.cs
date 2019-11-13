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

		private const ulong KilobyteSize = Denomination;
		private const ulong MegabyteSize = KilobyteSize * Denomination;
		private const ulong GigabyteSize = MegabyteSize * Denomination;
		private const ulong TerrabyteSize = GigabyteSize * Denomination;

		private const string ByteSuffix = "b";
		private const string KilobyteSuffix = "kB";
		private const string MegabyteSuffix = "MB";
		private const string GigabyteSuffix = "GB";

		public ulong Bytes { get; set; }

		public double Kilobytes
		{
			get
			{
				return ((double)Bytes) / KilobyteSize;
			}
			set
			{
				Bytes = (ulong)(value * KilobyteSize);
			}
		}

		public double Megabytes
		{
			get
			{
				return ((double)Bytes) / MegabyteSize;
			}
			set
			{
				Bytes = (ulong)(value * MegabyteSize);
			}
		}

		public double Gigabytes
		{
			get
			{
				return ((double)Bytes) / GigabyteSize;
			}
			set
			{
				Bytes = (ulong)(value * GigabyteSize);
			}
		}

		public double Terrabytes
		{
			get
			{
				return ((double)Bytes) / TerrabyteSize;
			}
			set
			{
				Bytes = (ulong)(value * TerrabyteSize);
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
						return $"{Bytes.ToString ()} {ByteSuffix}";
					}
					return $"{Kilobytes.ToString ()} {KilobyteSuffix}";
				}
				return $"{Megabytes.ToString ()} {MegabyteSuffix}";
			}
			return $"{Gigabytes.ToString ()} {GigabyteSuffix}";
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
						return $"{Bytes.ToString (format, formatProvider)} {ByteSuffix}";
					}
					return $"{Kilobytes.ToString (format, formatProvider)} {KilobyteSuffix}";
				}
				return $"{Megabytes.ToString (format, formatProvider)} {MegabyteSuffix}";
			}
			return $"{Gigabytes.ToString (format, formatProvider)} {GigabyteSuffix}";
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
