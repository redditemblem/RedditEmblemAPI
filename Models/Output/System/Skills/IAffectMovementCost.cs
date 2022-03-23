using RedditEmblemAPI.Models.Output.Units;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public interface IAffectMovementCost
    {
        bool IsActive(Unit tileUnit, Unit movingUnit);

        int GetMovementCost();
    }
}
