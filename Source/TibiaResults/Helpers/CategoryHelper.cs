using TibiaResults.Models;

namespace TibiaResults.Helpers
{
    internal static class CategoryHelper
    {
        public static readonly Category Experience = new(7, "experience", "Experience");

        public static IEnumerable<Category> GetNonExperienceCategories()
        {
            yield return new(1, "achievements", "Achievements");
            yield return new(2, "axefighting", "Axe Fighting");
            yield return new(3, "charmpoints", "Charm Points");
            yield return new(4, "clubfighting", "Club Fighting");
            yield return new(5, "distancefighting", "Distance Fighting");
            yield return new(6, "dromescore", "Drome Score");
            yield return new(8, "fishing", "Fishing");
            yield return new(9, "fistfighting", "Fist Fighting");
            yield return new(10, "goshnarstaint", "Goshnar's Taint");
            yield return new(11, "loyaltypoints", "Loyalty Points");
            yield return new(12, "magiclevel", "Magic Level");
            yield return new(13, "shielding", "Shielding");
            yield return new(14, "swordfighting", "Sword Fighting");
        }
    }
}
