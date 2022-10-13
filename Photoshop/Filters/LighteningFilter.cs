namespace MyPhotoshop
{
	public class LighteningFilter : PixelFilter<LighteningParameters>
	{
		public override string ToString ()
		{
			return "Осветление/затемнение";
		}
		
		public Pixel ProcessPixel(Pixel pixel, double[] parameters)
		{
			return pixel * parameters[0];
		}
		
		public override Pixel ProcessPixel(Pixel pixel, LighteningParameters parameters)
		{
			return pixel * parameters.Coefficient;
		}
	}
}

