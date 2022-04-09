using TibiaResults.Models;

namespace TibiaResults.Interfaces
{
    internal interface ILevelTrackingService
    {
        void UpdateLevelTracker(ILevelTracker levelTracker, IEnumerable<HighscoreEntry>? oldHighscore, IEnumerable<HighscoreEntry>? newHighscore);
    }
}
