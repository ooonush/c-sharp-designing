using System;
using System.Drawing;
using MyPhotoshop.Filters;

namespace MyPhotoshop;

public class TransformFilter<TParameters> : ParametrizedFilter<TParameters> where TParameters : IParameters, new()
{
    private readonly ITransformer<TParameters> _transformer;

    public TransformFilter(string name, ITransformer<TParameters> transformer) : base(name)
    {
        _transformer = transformer;
    }

    public override Photo Process(Photo original, TParameters parameters)
    {
        _transformer.Prepare(new Size(original.Width, original.Height), parameters);
        
        var result = new Photo(_transformer.ResultSize.Width, _transformer.ResultSize.Height);

        for (int x = 0; x < result.Width; x++)
        {
            for (int y = 0; y < result.Height; y++)
            {
                var oldPoint = _transformer.MapPoint(new Point(x, y));
                if (oldPoint != null)
                {
                    result[x, y] = original[oldPoint.Value.X, oldPoint.Value.Y];
                }
            }
        }

        return result;
    }
}

public class TransformFilter : TransformFilter<EmptyParameters>
{
    public TransformFilter(string name, Func<Size, Size> sizeTransformer, Func<Point, Size, Point> pointTransformer) 
        : base(name, new FreeTransformer(sizeTransformer, pointTransformer))
    {
    }
}