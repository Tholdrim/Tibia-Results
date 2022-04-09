using TibiaResults.Helpers;
using TibiaResults.Interfaces;
using TibiaResults.Models;

namespace TibiaResults.Services
{
    internal class ResultComputingService : IResultComputingService
    {
        private readonly IConfiguration _configuration;

        public ResultComputingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CategoryResult ComputeCategoryResult(IEnumerable<HighscoreEntry>? oldHighscore, IEnumerable<HighscoreEntry>? newHighscore)
        {
            return ComputeCategoryResult(oldHighscore, newHighscore, () => GetResultEntries(oldHighscore!, newHighscore!));
        }

        public CategoryResult ComputeExperienceCategoryResult(IEnumerable<HighscoreEntry>? oldHighscore, IEnumerable<HighscoreEntry>? newHighscore, ILevelTracker levelTracker)
        {
            return ComputeCategoryResult(oldHighscore, newHighscore, () => GetExperienceResultEntries(oldHighscore!, newHighscore!, levelTracker));
        }

        private IEnumerable<CategoryResultEntry> GetExperienceResultEntries(IEnumerable<HighscoreEntry> oldHighscore, IEnumerable<HighscoreEntry> newHighscore, ILevelTracker levelTracker)
        {
            foreach (var character in _configuration.Characters)
            {
                var characterLevels = levelTracker.GetCharacterLevels(character);
                var newEntry = newHighscore?.Where(e => e.Name == character).SingleOrDefault();

                if (newEntry == null && !characterLevels.New.HasValue)
                {
                    continue;
                }

                var isApproximate = false;
                var newExperience = newEntry?.Value;

                if (!newExperience.HasValue)
                {
                    isApproximate = true;
                    newExperience = ExperienceHelper.ForLevel(characterLevels.New!.Value);
                }

                var oldExperience = oldHighscore?.Where(e => e.Name == character).Select(e => (long?)e.Value).SingleOrDefault();

                if (oldExperience == null && characterLevels.Old.HasValue)
                {
                    isApproximate = true;
                    oldExperience = ExperienceHelper.ForLevel(characterLevels.Old.Value);
                }

                var progress = oldExperience.HasValue ? newExperience - oldExperience.Value : null;

                yield return new CategoryResultEntry(newEntry?.Rank, character, newExperience.Value, progress, isApproximate);
            }
        }

        private IEnumerable<CategoryResultEntry> GetResultEntries(IEnumerable<HighscoreEntry> oldHighscore, IEnumerable<HighscoreEntry> newHighscore)
        {
            foreach (var character in _configuration.Characters)
            {
                var newEntry = newHighscore?.Where(e => e.Name == character).SingleOrDefault();

                if (newEntry == null)
                {
                    continue;
                }

                var oldValue = oldHighscore?.Where(e => e.Name == character).Select(e => (long?)e.Value).SingleOrDefault();
                var progress = oldValue.HasValue ? newEntry.Value - oldValue.Value : (long?)null;

                yield return new CategoryResultEntry(newEntry.Rank, newEntry.Name, newEntry.Value, progress);
            }
        }

        private static CategoryResult ComputeCategoryResult(IEnumerable<HighscoreEntry>? oldHighscore, IEnumerable<HighscoreEntry>? newHighscore, Func<IEnumerable<CategoryResultEntry>> resultEntriesGettingDelegate)
        {
            if (oldHighscore == null || newHighscore == null)
            {
                return CategoryResult.CreateNotAvailable();
            }

            var resultEntries = resultEntriesGettingDelegate().ToList();

            return CategoryResult.Create(resultEntries);
        }
    }
}
