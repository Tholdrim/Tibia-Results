using TibiaResults.Models;

namespace TibiaResults.Interfaces
{
    internal interface IHighscoreProvider
    {
        Task<IEnumerable<HighscoreEntry>?> GetHighscoreAsync(string identifier, DateOnly date);
    }
}
