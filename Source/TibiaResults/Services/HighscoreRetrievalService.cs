using TibiaResults.Exceptions;
using TibiaResults.Interfaces;
using TibiaResults.Models;
using TibiaResults.Providers;

namespace TibiaResults.Services
{
    internal class HighscoreRetrievalService : IHighscoreRetrievalService
    {
        private readonly IConfigurationService _configurationService;

        public HighscoreRetrievalService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;

            Providers = new Lazy<IEnumerable<IHighscoreProvider>>(InitializeProviders);
        }

        private Lazy<IEnumerable<IHighscoreProvider>> Providers { get; }

        public Task<IEnumerable<HighscoreEntry>?> GetOldHighscoreAsync(string identifier) => GetHighscoreAsync(identifier, _configurationService.Dates.From);

        public Task<IEnumerable<HighscoreEntry>?> GetNewHighscoreAsync(string identifier) => GetHighscoreAsync(identifier, _configurationService.Dates.To);

        private async Task<IEnumerable<HighscoreEntry>?> GetHighscoreAsync(string identifier, DateOnly date)
        {
            foreach (var provider in Providers.Value)
            {
                var highscore = await TryToGetHighscoreAsync(provider, identifier, date);

                if (highscore?.HighscoreList == null)
                {
                    continue;
                }

                var relevantEntries = GetRelevantEntries(highscore.HighscoreList);

                return relevantEntries.ToList();
            }

            return null;
        }

        private IEnumerable<HighscoreEntry> GetRelevantEntries(IEnumerable<HighscoreEntry> entries)
        {
            foreach (var character in _configurationService.Characters)
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
            var providers = GetProviders(_configurationService.BlobContainerUri, _configurationService.LocalPath);

            return providers.ToList();
        }

        private static IEnumerable<IHighscoreProvider> GetProviders(Uri? blobContainerUri, string? localPath)
        {
            if (localPath != null)
            {
                yield return new FileProvider(localPath);
            }

            if (blobContainerUri != null)
            {
                yield return new AzureBlobProvider(blobContainerUri);
            }
        }

        private static async Task<Highscore?> TryToGetHighscoreAsync(IHighscoreProvider provider, string identifier, DateOnly date)
        {
            try
            {
                return await provider.GetHighscoreAsync(identifier, date);
            }
            catch (Exception exception)
            {
                throw new HighscoreRetrievalException(provider.GetType(), identifier, date, exception);
            }
        }
    }
}
