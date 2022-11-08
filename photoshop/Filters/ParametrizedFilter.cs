namespace MyPhotoshop;

public abstract class ParametrizedFilter<TParameters> : IFilter where TParameters : IParameters, new()
{
    private readonly string _name;

    protected ParametrizedFilter(string name)
    {
        _name = name;
    }

    public ParameterInfo[] GetParameters()
    {
        return new TParameters().GetDescription();
    }
    
    public Photo Process(Photo original, double[] values)
    {
        var parameters = new TParameters();
        parameters.SetValues(values);
        return Process(original, parameters);
    }
    
    public abstract Photo Process(Photo original, TParameters parameters);

    public override string ToString()
    {
        return _name;
    }
}