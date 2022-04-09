namespace TibiaResults.Exceptions
{
    internal class HighscoreRetrievalException : Exception
    {
        public HighscoreRetrievalException(Type highscoreProviderType, string identifier, DateOnly date, Exception innerException)
            : base($"Failed to retrieve the {identifier}/{date:yyyy-MM-dd}.json file using {highscoreProviderType.Name}. The data source is probably inaccessible or corrupt.", innerException)
        {
        }
    }
}
