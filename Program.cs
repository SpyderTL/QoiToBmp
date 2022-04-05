namespace QoiToBmp
{
	public static class Program
	{
		public static int Main(string[] arguments)
		{
			Arguments.Parse(arguments);

			if (!Arguments.Valid)
				return 1;

			BinaryFileReader.Open(Arguments.Source);

			QoiFileReader.Read();

			QoiImageReader.Read();

			BitmapImage.SaveImage();

			BitmapFile.Save(Arguments.Destination);

			return 0;
		}
	}
}