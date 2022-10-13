using System;

namespace MyPhotoshop
{
    public class GrayscaleFilter : PixelFilter
    {
        public GrayscaleFilter() : base(new EmptyParameters())
        {
        }

        public override string ToString()
        {
            return "Оттенки серого";
        }

        public override Pixel ProcessPixel(Pixel pixel, IParameters parameters)
        {
            double lightness = 0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B;
            return new Pixel(lightness, lightness, lightness);
        }
    }
}