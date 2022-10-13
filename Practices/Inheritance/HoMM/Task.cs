namespace Inheritance.MapObjects
{
    public interface IOwnerable
    {
        int Owner { get; set; }
    }

    public interface IArmyable
    {
        Army Army { get; set; }
    }

    public interface ITreasureable
    {
        Treasure Treasure { get; set; }
    }

    public class Dwelling : IOwnerable
    {
        public int Owner { get; set; }
    }

    public class Mine : IOwnerable, IArmyable, ITreasureable
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Creeps : IArmyable, ITreasureable
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Wolves : IArmyable
    {
        public Army Army { get; set; }
    }

    public class ResourcePile : ITreasureable
    {
        public Treasure Treasure { get; set; }
    }

    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
            if (mapObject is IArmyable armyable)
            {
                if (!player.CanBeat(armyable.Army))
                {
                    player.Die();
                    return;
                }
            }
            
            if (mapObject is ITreasureable treasureable)
            {
                player.Consume(treasureable.Treasure);
            }
            if (mapObject is IOwnerable ownerable)
            {
                ownerable.Owner = player.Id;
            }
        }
    }
}
