using System;
using System.Drawing;

namespace MyPhotoshop.Filters;

public class FreeTransformer : ITransformer<EmptyParameters>
{
    public Size ResultSize { get; private set; }
    private readonly Func<Size, Size> _sizeTransformer;
    private readonly Func<Point, Size, Point> _pointTransformer;
    private Size _oldSize;

    public FreeTransformer(Func<Size, Size> sizeTransformer, Func<Point, Size, Point> pointTransformer)
    {
        _sizeTransformer = sizeTransformer;
        _pointTransformer = pointTransformer;
    }

    public void Prepare(Size size, EmptyParameters parameters)
    {
        _oldSize = size;
        ResultSize = _sizeTransformer(size);
    }

    public Point? MapPoint(Point newPoint)
    {
        return _pointTransformer(newPoint, _oldSize);
    }
}