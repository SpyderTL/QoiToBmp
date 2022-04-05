using System.Drawing;

namespace QoiToBmp
{
	internal class QoiImageReader
	{
		internal static void Read()
		{
			Image.Width = QoiFile.Width;
			Image.Height = QoiFile.Height;

			var colors = new List<Color>();
			var index = Enumerable.Repeat((Color?)null, 64).ToArray();
			var last = Color.Black;

			foreach (var block in QoiFile.Blocks)
			{
				switch (block.Type)
				{
					case QoiBlockType.Rgb:
						last = Color.FromArgb(last.A, block.Red, block.Green, block.Blue);
						colors.Add(last);

						var hash = Hash(last);

						index[hash] = last;

						break;

					case QoiBlockType.Rgba:
						last = Color.FromArgb(block.Alpha, block.Red, block.Green, block.Blue);
						colors.Add(last);

						hash = Hash(last);

						index[hash] = last;

						break;

					case QoiBlockType.Run:
						colors.AddRange(Enumerable.Repeat(last, block.Run + 1));
						break;

					case QoiBlockType.Index:
						var value = index[block.Index];

						if (value.HasValue)
						{
							last = index[block.Index].Value;

							colors.Add(last);
						}
						else
						{
							throw new Exception("Index not found");
						}
						break;

					case QoiBlockType.Diff:
						last = Color.FromArgb(
							last.A,
							(last.R + (block.Red - 2) + 256) % 256,
							(last.G + (block.Green - 2) + 256) % 256,
							(last.B + (block.Blue - 2) + 256) % 256);

						colors.Add(last);

						hash = Hash(last);

						index[hash] = last;
						break;

					case QoiBlockType.Luma:
						last = Color.FromArgb(
							last.A,
							(last.R + (block.Green - 32) + (block.Red - 8) + 256) % 256,
							(last.G + (block.Green - 32) + 256) % 256,
							(last.B + (block.Green - 32) + (block.Blue - 8) + 256) % 256);

						colors.Add(last);

						hash = Hash(last);

						index[hash] = last;
						break;

					default:
						throw new Exception("Unknown block type");
				}
			}

			Image.Colors = colors.ToArray();
		}

		private static int Hash(Color color)
		{
			return
				(color.R * 3 +
				color.G * 5 +
				color.B * 7 +
				color.A * 11) % 64;
		}
	}
}