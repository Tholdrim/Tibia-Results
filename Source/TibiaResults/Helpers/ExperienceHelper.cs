namespace TibiaResults.Helpers
{
    internal static class ExperienceHelper
    {
        public static long ForLevel(long level) => 50 * (level * level * level - 6 * level * level + 17 * level - 12) / 3;
    }
}
