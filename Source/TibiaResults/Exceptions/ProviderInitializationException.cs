namespace TibiaResults.Exceptions
{
    internal class ProviderInitializationException<IHighscoreProviderType> : Exception
    {
        public ProviderInitializationException(Exception innerException)
            : base($"Failed to initialize {typeof(IHighscoreProviderType).Name}. Please check the application settings.", innerException)
        {
        }
    }
}
