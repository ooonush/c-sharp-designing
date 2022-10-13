using System;

namespace MyPhotoshop;

public class EmptyParameters : IParameters
{
    public ParameterInfo[] GetDescription()
    {
        return Array.Empty<ParameterInfo>();
    }

    public void SetValues(double[] values)
    {
        
    }
}