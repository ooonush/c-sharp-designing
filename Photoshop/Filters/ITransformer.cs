using System.Drawing;

namespace MyPhotoshop.Filters;

public interface ITransformer<in TParameters> where TParameters : IParameters
{
    Size ResultSize { get; }
    void Prepare(Size size, TParameters parameters);
    Point? MapPoint(Point newPoint);
}