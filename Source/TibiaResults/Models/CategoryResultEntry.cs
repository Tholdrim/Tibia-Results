namespace TibiaResults.Models
{

    internal record CategoryResultEntry(int? Rank, string Name, long Value, long? Progress, bool IsApproximate = false);
}
