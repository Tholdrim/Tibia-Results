namespace TibiaResults.Exceptions
{
    internal class ConfigurationFileException : Exception
    {
        public ConfigurationFileException(string fileName)
            : base($"The {fileName} file could not be found.")
        {
        }
    }
}
