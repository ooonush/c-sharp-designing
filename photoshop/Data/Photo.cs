namespace MyPhotoshop
{
	public class Photo
	{
		public readonly int Width;
		public readonly int Height;
		private readonly Pixel[,] _pixels;

		public Pixel this[int x, int  y]
		{
			get => _pixels[x, y];
			set => _pixels[x, y] = value;
		}

		public Photo(int width, int height)
		{
			Width = width;
			Height = height;
			_pixels = new Pixel[width, height];
		}
	}
}