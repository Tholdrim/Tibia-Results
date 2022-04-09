using TibiaResults.Interfaces;
using TibiaResults.Models;

namespace TibiaResults.Services
{
    internal class LevelTrackingService : ILevelTrackingService
    {
        private readonly IConfiguration _configuration;

        public LevelTrackingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void UpdateLevelTracker(ILevelTracker levelTracker, IEnumerable<HighscoreEntry>? oldHighscore, IEnumerable<HighscoreEntry>? newHighscore)
        {
            foreach (var character in _configuration.Characters)
            {
                var characterLevelHistory = levelTracker.GetCharacterLevels(character);
                var oldHighscoreLevel = oldHighscore?.Where(e => e.Name == character).Select(e => (int?)e.Level).SingleOrDefault();

                if (oldHighscoreLevel.HasValue && (!characterLevelHistory.Old.HasValue || oldHighscoreLevel.Value > characterLevelHistory.Old.Value))
                {
                    characterLevelHistory = characterLevelHistory with { Old = oldHighscoreLevel };
                }

                var newHighscoreLevel = newHighscore?.Where(e => e.Name == character).Select(e => (int?)e.Level).SingleOrDefault();

                if (newHighscoreLevel.HasValue && (!characterLevelHistory.New.HasValue || newHighscoreLevel.Value > characterLevelHistory.New.Value))
                {
                    characterLevelHistory = characterLevelHistory with { New = newHighscoreLevel };
                }

                levelTracker.Update(character, characterLevelHistory);
            }
        }
    }
}
