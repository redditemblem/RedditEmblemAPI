using RedditEmblemAPI.Models.Output.Units;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public interface IAffectMovementCost
    {
        bool IsActive(IUnit tileUnit, IUnit movingUnit);

        int GetMovementCost();
    }
}
