using Azure.Storage.Blobs;
using System.Text.Json;
using TibiaResults.Interfaces;
using TibiaResults.Models;

namespace TibiaResults.Providers
{
    internal class AzureBlobProvider : IHighscoreProvider
    {
        private readonly BlobContainerClient _blobContainerClient;

        public AzureBlobProvider(Uri blobContainerUri)
        {
            _blobContainerClient = new BlobContainerClient(blobContainerUri);
        }

        public Task<IEnumerable<HighscoreEntry>?> GetHighscoreAsync(string identifier, DateOnly date) => DownloadHighscoreAsync(identifier, date);

        private async Task<IEnumerable<HighscoreEntry>?> DownloadHighscoreAsync(string identifier, DateOnly date)
        {
            var blob = _blobContainerClient.GetBlobClient($"{identifier}/{date:yyyy-MM-dd}.json");
            var blobExists = await blob.ExistsAsync();

            if (!blobExists)
            {
                return null;
            }

            var streamingResponse = await blob.DownloadStreamingAsync();
            var highscoresRoot = await JsonSerializer.DeserializeAsync<HighscoreRoot>(streamingResponse.Value.Content);

            return highscoresRoot?.Highscores?.HighscoreList;
        }
    }
}
