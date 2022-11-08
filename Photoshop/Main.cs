using System;
using System.Drawing;
using System.Windows.Forms;
using MyPhotoshop.Filters;

namespace MyPhotoshop
{
	class MainClass
	{
		[STAThread]
		public static void Main(string[] args)
		{
			var window = new MainWindow();
			window.AddFilter(new PixelFilter<LighteningParameters>(
				"Осветление/затемнение",
				(pixel, parameters) => pixel * parameters.Coefficient));
			window.AddFilter(new PixelFilter<EmptyParameters>(
				"Оттенки серого",
				(pixel, _) =>
				{
					double lightness = 0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B;
					return new Pixel(lightness, lightness, lightness);
				}));

			window.AddFilter(new TransformFilter<RotationParameters>("Свободное вращение", new RotateTransformer()));
			window.AddFilter(new TransformFilter(
				"Отразить по горизонтали",
				size => size,
				(point, size) => point with { X = size.Width - point.X - 1 })
			);
			window.AddFilter(new TransformFilter(
				"Повернуть по ч.с.",
				size => new Size(size.Height, size.Width),
				(point, _) => new Point(point.Y, point.X))
			);
			window.AddFilter(new TransformFilter<RotationParameters>("Свободное вращение", new RotateTransformer()));
			Application.Run(window);
		}
	}
}
