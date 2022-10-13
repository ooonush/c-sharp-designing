namespace Inheritance.MapObjects
{
    public interface IArmy
    {
        int Power { get; set; }
    }

    public class Army : IArmy
    {
        public int Power { get; set; }
    }
}
