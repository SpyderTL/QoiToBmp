using System.Drawing;

namespace QoiToBmp
{
	internal class QoiFileReader
	{
		private static QoiBlock Block;
		private static ulong Length;
		private static ulong Position;

		internal static void Read()
		{
			ReadHeader();

			ReadBlocks();
		}

		internal static void ReadHeader()
		{
			if (BinaryFileReader.Reader == null)
				return;

			QoiFile.Signature = BinaryFileReader.Reader.ReadChars(4);

			QoiFile.Width = (uint)((BinaryFileReader.Reader.ReadByte() << 24) |
				(BinaryFileReader.Reader.ReadByte() << 16) |
				(BinaryFileReader.Reader.ReadByte() << 8) |
				(BinaryFileReader.Reader.ReadByte() << 0));

			QoiFile.Height = (uint)((BinaryFileReader.Reader.ReadByte() << 24) |
				(BinaryFileReader.Reader.ReadByte() << 16) |
				(BinaryFileReader.Reader.ReadByte() << 8) |
				(BinaryFileReader.Reader.ReadByte() << 0));

			QoiFile.Channels = BinaryFileReader.Reader.ReadByte();
			QoiFile.Colorspace = BinaryFileReader.Reader.ReadByte();

			Length = QoiFile.Width * QoiFile.Height;

			Position = 0;
		}

		internal static bool ReadBlock()
		{
			if (Position >= Length)
				return false;

			var value = BinaryFileReader.Reader.ReadByte();

			if (value == (byte)QoiBlockType.Rgba)
			{
				Block.Type = QoiBlockType.Rgba;
				Block.Red = BinaryFileReader.Reader.ReadByte();
				Block.Green = BinaryFileReader.Reader.ReadByte();
				Block.Blue = BinaryFileReader.Reader.ReadByte();
				Block.Alpha = BinaryFileReader.Reader.ReadByte();

				Position++;
			}
			else if (value == (byte)QoiBlockType.Rgb)
			{
				Block.Type = QoiBlockType.Rgb;
				Block.Red = BinaryFileReader.Reader.ReadByte();
				Block.Green = BinaryFileReader.Reader.ReadByte();
				Block.Blue = BinaryFileReader.Reader.ReadByte();

				Position++;
			}
			else
			{
				var type = value & 0xC0;

				switch (type)
				{
					case (byte)QoiBlockType.Diff:
						Block.Type = QoiBlockType.Diff;
						Block.Red = (byte)((value >> 4) & 0x03);
						Block.Green = (byte)((value >> 2) & 0x03);
						Block.Blue = (byte)((value >> 0) & 0x03);

						Position++;
						break;

					case (byte)QoiBlockType.Luma:
						Block.Type = QoiBlockType.Luma;
						Block.Green = (byte)(value & 0x3F);
						value = BinaryFileReader.Reader.ReadByte();
						Block.Red = (byte)(value >> 4);
						Block.Blue = (byte)(value & 0x0F);
						Position++;
						break;

					case (byte)QoiBlockType.Run:
						Block.Type = QoiBlockType.Run;
						Block.Run = (byte)(value & 0x3F);
						Position += (ulong)Block.Run + 1;
						break;

					default:
						Block.Type = QoiBlockType.Index;
						Block.Index = (byte)(value & 0x3F);
						Position++;
						break;
				}
			}

			return true;
		}

		internal static void ReadBlocks()
		{
			var blocks = new List<QoiBlock>();

			while (ReadBlock())
				blocks.Add(Block);

			QoiFile.Blocks = blocks.ToArray();
		}
	}
}