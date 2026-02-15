using RedditEmblemAPI.Models.Output.Units;

namespace RedditEmblemAPI.Models.Output.System.Skills.Effects
{
    public interface IIgnoreUnitAffiliations
    {
        bool IsActive(IUnit unit);
    }
}
