namespace MyPhotoshop
{
	public class LighteningFilter : PixelFilter
	{
		public LighteningFilter() : base(new LighteningParameters()) { }
		
		public override string ToString ()
		{
			return "Осветление/затемнение";
		}
		
		public Pixel ProcessPixel(Pixel pixel, double[] parameters)
		{
			return pixel * parameters[0];
		}
		
		public override Pixel ProcessPixel(Pixel pixel, IParameters parameters)
		{
			return pixel * ((LighteningParameters)parameters).Coefficient;
		}
	}
}

