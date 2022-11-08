namespace MyPhotoshop;

public class RotationParameters : IParameters
{
    public double Angle { get; private set; }
    
    public ParameterInfo[] GetDescription()
    {
        return new[]
        {
            new ParameterInfo()
            {
                Name = "Угол",
                MaxValue = 360,
                MinValue = 0,
                Increment = 5,
                DefaultValue = 0
            }
        };
    }

    public void SetValues(double[] values)
    {
        Angle = values[0];
    }
}