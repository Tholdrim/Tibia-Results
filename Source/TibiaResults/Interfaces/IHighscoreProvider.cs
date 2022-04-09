using TibiaResults.Models;

namespace TibiaResults.Interfaces
{
    internal interface IHighscoreProvider
    {
        Task<Highscore?> GetHighscoreAsync(string identifier, DateOnly date);
    }
}
