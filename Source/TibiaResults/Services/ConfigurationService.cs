using TibiaResults.Exceptions;
using TibiaResults.Helpers;
using TibiaResults.Interfaces;
using TibiaResults.Models;

namespace TibiaResults.Services
{
    internal class ConfigurationService : IConfigurationService
    {
        public ConfigurationService()
        {
            var applicationSettings = GetApplicationSettings();

            BlobContainerUri = GetBlobContainerUri(applicationSettings);
            LocalPath = GetLocalPath(applicationSettings);
            Characters = GetCharacters(applicationSettings);
            Dates = GetDates(applicationSettings);
        }

        public Uri? BlobContainerUri { get; }

        public string? LocalPath { get; }

        public IEnumerable<string> Characters { get; } = null!;

        public (DateOnly From, DateOnly To) Dates { get; }

        private static ApplicationSettings GetApplicationSettings()
        {
            const string fileName = "Settings.json";

            try
            {
                var applicationSettings = FileHelper.DeserializeFromFile<ApplicationSettings>(fileName);

                return applicationSettings ?? throw new ConfigurationFileException(fileName);
            }
            catch (Exception exception) when (exception is not ConfigurationFileException)
            {
                throw new ConfigurationException("Invalid JSON file format.", exception);
            }
        }

        private static Uri? GetBlobContainerUri(ApplicationSettings applicationSettings)
        {
            if (applicationSettings.BlobContainerUri == null)
            {
                return null;
            }

            if (!Uri.TryCreate(applicationSettings.BlobContainerUri, UriKind.Absolute, out var blobContainerUri))
            {
                throw new ConfigurationException("The specified 'blobContainerUri' value is not a valid URL.");
            }

            return blobContainerUri;
        }

        private static string? GetLocalPath(ApplicationSettings applicationSettings)
        {
            if (applicationSettings.LocalPath == null)
            {
                return null;
            }

            if (!Directory.Exists(applicationSettings.LocalPath))
            {
                throw new ConfigurationException("The directory specified in the 'localPath' field does not exist.");
            }

            return applicationSettings.LocalPath;
        }

        private static IEnumerable<string> GetCharacters(ApplicationSettings applicationSettings)
        {
            if (applicationSettings.Characters == null || !applicationSettings.Characters.Any())
            {
                throw new ConfigurationException("No character was specified.");
            }

            return applicationSettings.Characters;
        }

        private static (DateOnly From, DateOnly To) GetDates(ApplicationSettings applicationSettings)
        {
            var fromDate = GetDate(applicationSettings.From, nameof(applicationSettings.From));
            var toDate = GetDate(applicationSettings.To, nameof(applicationSettings.To));

            return (fromDate, toDate);
        }

        private static DateOnly GetDate(string? input, string fieldName)
        {
            const string dateFormat = "yyyy-MM-dd";

            if (input == null)
            {
                throw new ConfigurationException($"The '{fieldName.ToLower()}' date is required and cannot be omitted.");
            }

            if (!DateOnly.TryParseExact(input, dateFormat, out var date))
            {
                throw new ConfigurationException($"Invalid date in the '{fieldName.ToLower()}' field - required format is YYYY-MM-DD.");
            }

            return date;
        }
    }
}
