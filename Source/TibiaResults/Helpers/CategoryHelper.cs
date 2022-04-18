using TibiaResults.Consts;
using TibiaResults.Models;

namespace TibiaResults.Helpers
{
    internal static class CategoryHelper
    {
        public static IEnumerable<Category> GetCategories()
        {
            yield return Categories.Achievements;
            yield return Categories.AxeFighting;
            yield return Categories.CharmPoints;
            yield return Categories.ClubFighting;
            yield return Categories.DistanceFighting;
            yield return Categories.DromeScore;
            yield return Categories.Experience;
            yield return Categories.Fishing;
            yield return Categories.FistFighting;
            yield return Categories.GoshnarsTaint;
            yield return Categories.LoyaltyPoints;
            yield return Categories.MagicLevel;
            yield return Categories.Shielding;
            yield return Categories.SwordFighting;
        }
    }
}
