using TibiaResults.Models;

namespace TibiaResults.Interfaces
{
    internal interface IResultComputingService
    {
        CategoryResult ComputeCategoryResult(IEnumerable<HighscoreEntry>? oldHighscore, IEnumerable<HighscoreEntry>? newHighscore);

        CategoryResult ComputeExperienceCategoryResult(IEnumerable<HighscoreEntry>? oldHighscore, IEnumerable<HighscoreEntry>? newHighscore, ILevelTracker levelTracker);
    }
}
