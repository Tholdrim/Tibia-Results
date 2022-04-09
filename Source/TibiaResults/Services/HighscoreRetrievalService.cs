using TibiaResults.Exceptions;
using TibiaResults.Interfaces;
using TibiaResults.Models;
using TibiaResults.Providers;

namespace TibiaResults.Services
{
    internal class HighscoreRetrievalService : IHighscoreRetrievalService
    {
        private readonly IConfiguration _configuration;

        public HighscoreRetrievalService(IConfiguration configuration)
        {
            _configuration = configuration;

            Providers = new Lazy<IEnumerable<IHighscoreProvider>>(() => InitializeProviders(configuration));
        }

        private Lazy<IEnumerable<IHighscoreProvider>> Providers { get; }

        public Task<IEnumerable<HighscoreEntry>?> GetOldHighscoreAsync(string identifier) => GetHighscoreAsync(identifier, _configuration.Dates.From);

        public Task<IEnumerable<HighscoreEntry>?> GetNewHighscoreAsync(string identifier) => GetHighscoreAsync(identifier, _configuration.Dates.To);

        private async Task<IEnumerable<HighscoreEntry>?> GetHighscoreAsync(string identifier, DateOnly date)
        {
            foreach (var provider in Providers.Value)
            {
                var highscore = await TryToGetHighscoreAsync(provider, identifier, date);

                if (highscore != null)
                {
                    return GetRelevantEntries(highscore);
                }
            }

            return null;
        }

        private IEnumerable<HighscoreEntry> GetRelevantEntries(IEnumerable<HighscoreEntry> entries)
        {
            var result = new List<HighscoreEntry>();

            foreach (var character in _configuration.Characters)
            {
                var characterEntry = entries.SingleOrDefault(e => e.Name == character);

                if (characterEntry == null)
                {
                    continue;
                }

                characterEntry.Rank = entries.Where(e => e.Value == characterEntry.Value).Min(e => e.Rank);

                result.Add(characterEntry);
            }

            return result;
        }

        private static IEnumerable<IHighscoreProvider> InitializeProviders(IConfiguration configuration)
        {
            if (configuration.LocalPath != null)
            {
                yield return TryToCreateProvider(() => new FileProvider(configuration.LocalPath));
            }

            if (configuration.BlobContainerUri != null)
            {
                yield return TryToCreateProvider(() => new AzureBlobProvider(configuration.BlobContainerUri));
            }
        }

        private static T TryToCreateProvider<T>(Func<T> @delegate) where T : class
        {
            try
            {
                return @delegate();
            }
            catch (Exception exception)
            {
                throw new ProviderInitializationException<T>(exception);
            }
        }

        private static async Task<IEnumerable<HighscoreEntry>?> TryToGetHighscoreAsync(IHighscoreProvider provider, string identifier, DateOnly date)
        {
            try
            {
                var highscore = await provider.GetHighscoreAsync(identifier, date);

                return highscore;
            }
            catch (Exception exception)
            {
                throw new HighscoreRetrievalException(provider.GetType(), identifier, date, exception);
            }
        }
    }
}
