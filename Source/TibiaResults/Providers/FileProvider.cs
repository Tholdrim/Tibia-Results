using System.Text.Json;
using TibiaResults.Interfaces;
using TibiaResults.Models;

namespace TibiaResults.Providers
{
    internal class FileProvider : IHighscoreProvider
    {
        public FileProvider(string localPath)
        {
            if (!Directory.Exists(localPath))
            {
                throw new InvalidOperationException("The specified directory does not exist.");
            }

            LocalPath = localPath;
        }

        private string LocalPath { get; }

        public Task<IEnumerable<HighscoreEntry>?> GetHighscoreAsync(string identifier, DateOnly date) => ReadHighscoreAsync(identifier, date);

        private async Task<IEnumerable<HighscoreEntry>?> ReadHighscoreAsync(string identifier, DateOnly date)
        {
            var filePath = Path.Combine(LocalPath, identifier, $"{date:yyyy-MM-dd}.json");
            var fileExists = File.Exists(filePath);

            if (!fileExists)
            {
                return null;
            }
            
            var fileStream = File.OpenRead(filePath);
            var highscoresRoot = await JsonSerializer.DeserializeAsync<HighscoreRoot>(fileStream);

            return highscoresRoot?.Highscores?.HighscoreList;
        }
    }
}
