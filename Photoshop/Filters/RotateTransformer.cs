using System;
using System.Drawing;

namespace MyPhotoshop.Filters;

public class RotateTransformer : ITransformer<RotationParameters>
{
    public Size ResultSize { get; private set; }
    private Size OriginalSize { get; set; }
    private RotationParameters Parameters { get; set; }
    private static double Angle { get; set; }

    public void Prepare(Size size, RotationParameters parameters)
    {
        OriginalSize = size;
        Angle = Math.PI * parameters.Angle / 180;
        ResultSize = new Size(
            (int)(size.Width * Math.Abs(Math.Cos(Angle)) + size.Height * Math.Abs(Math.Sin(Angle))), 
            (int)(size.Height * Math.Abs(Math.Cos(Angle)) + size.Width * Math.Abs(Math.Sin(Angle)))
            );
        Parameters = parameters;
    }

    public Point? MapPoint(Point newPoint)
    {
        Size newSize = ResultSize;
        double angle = Math.PI * Parameters.Angle / 180;
        newPoint = new Point(newPoint.X - newSize.Width / 2, newPoint.Y - newSize.Height / 2);
        int x = OriginalSize.Width / 2 + (int)(newPoint.X * Math.Cos(angle) + newPoint.Y * Math.Sin(angle));
        int y = OriginalSize.Height / 2 + (int)(-newPoint.X * Math.Sin(angle) + newPoint.Y * Math.Cos(angle));
        if (x < 0 || x >= OriginalSize.Width || y < 0 || y >= OriginalSize.Height)
        {
            return null;
        }
        return new Point(x, y);
    }
}