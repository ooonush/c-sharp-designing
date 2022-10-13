namespace MyPhotoshop;

public abstract class ParametrizedFilter : IFilter
{
    private readonly IParameters _parameters;
    
    public ParametrizedFilter(IParameters parameters)
    {
        _parameters = parameters;
    }
    
    public ParameterInfo[] GetParameters()
    {
        return _parameters.GetDescription();
    }
    
    public Photo Process(Photo original, double[] parameters)
    {
        _parameters.SetValues(parameters);
        return Process(original, _parameters);
    }
    
    public abstract Photo Process(Photo original, IParameters parameters);
}