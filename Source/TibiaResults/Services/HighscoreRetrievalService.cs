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

            Providers = new Lazy<IEnumerable<IHighscoreProvider>>(InitializeProviders);
        }

        private Lazy<IEnumerable<IHighscoreProvider>> Providers { get; }

        public Task<IEnumerable<HighscoreEntry>?> GetOldHighscoreAsync(string identifier) => GetHighscoreAsync(identifier, _configuration.Dates.From);

        public Task<IEnumerable<HighscoreEntry>?> GetNewHighscoreAsync(string identifier) => GetHighscoreAsync(identifier, _configuration.Dates.To);

        private async Task<IEnumerable<HighscoreEntry>?> GetHighscoreAsync(string identifier, DateOnly date)
        {
            foreach (var provider in Providers.Value)
            {
                var highscore = await TryToGetHighscoreAsync(provider, identifier, date);

                if (highscore?.HighscoreList != null)
                {
                    var relevantEntries = GetRelevantEntries(highscore.HighscoreList);

                    return relevantEntries.ToList();
                }
            }

            return null;
        }

        private IEnumerable<HighscoreEntry> GetRelevantEntries(IEnumerable<HighscoreEntry> entries)
        {
            foreach (var character in _configuration.Characters)
            {
                var characterEntry = entries.SingleOrDefault(e => e.Name == character);

                if (characterEntry == null)
                {
                    continue;
                }

                characterEntry.Rank = entries.Where(e => e.Value == characterEntry.Value).Min(e => e.Rank);

                yield return characterEntry;
            }
        }

        private IEnumerable<IHighscoreProvider> InitializeProviders()
        {
            var providers = GetProviders(_configuration.BlobContainerUri, _configuration.LocalPath);

            return providers.ToList();
        }

        private static IEnumerable<IHighscoreProvider> GetProviders(Uri? blobContainerUri, string? localPath)
        {
            if (localPath != null)
            {
                yield return TryToCreateProvider(() => new FileProvider(localPath));
            }

            if (blobContainerUri != null)
            {
                yield return TryToCreateProvider(() => new AzureBlobProvider(blobContainerUri));
            }
        }

        private static T TryToCreateProvider<T>(Func<T> providerCreatingDelegate) where T : class
        {
            try
            {
                return providerCreatingDelegate();
            }
            catch (Exception exception)
            {
                throw new ProviderInitializationException<T>(exception);
            }
        }

        private static async Task<Highscore?> TryToGetHighscoreAsync(IHighscoreProvider provider, string identifier, DateOnly date)
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
