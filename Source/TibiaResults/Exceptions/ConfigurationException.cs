namespace TibiaResults.Exceptions
{
    internal class ConfigurationException : Exception
    {
        public ConfigurationException(string message, Exception? innerException = null)
            : base($"{message} Please check the application settings.", innerException)
        {
        }
    }
}
