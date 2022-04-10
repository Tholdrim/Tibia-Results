using Microsoft.Extensions.Configuration;
using TibiaResults.Models;

using IConfiguration = TibiaResults.Interfaces.IConfiguration;

namespace TibiaResults.Configuration
{
    internal class Configuration : IConfiguration
    {
        public Configuration()
        {
            var applicationSettings = GetApplicationSettings();

            Initialize(applicationSettings);
        }

        public Uri? BlobContainerUri { get; private set; }

        public string? LocalPath { get; private set; }

        public IEnumerable<string> Characters { get; private set; } = null!;

        public (DateOnly From, DateOnly To) Dates { get; private set; }

        private void Initialize(ApplicationSettings applicationSettings)
        {
            if (applicationSettings.Characters == null || !applicationSettings.Characters.Any())
            {
                throw new InvalidOperationException("No character was specified. Please check the application settings.");
            }

            if (!applicationSettings.From.HasValue || !applicationSettings.To.HasValue)
            {
                throw new InvalidOperationException("The 'from' and 'to' dates are required and cannot be omitted. Please check the application settings.");
            }

            var fromDate = DateOnly.FromDateTime(applicationSettings.From.Value);
            var toDate = DateOnly.FromDateTime(applicationSettings.To.Value);

            BlobContainerUri = applicationSettings.BlobContainerUri;
            LocalPath = applicationSettings.LocalPath;
            Characters = applicationSettings.Characters;
            Dates = (fromDate, toDate);
        }

        private static ApplicationSettings GetApplicationSettings()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("Settings.json")
                .Build();

            return configurationBuilder.Get<ApplicationSettings>();
        }
    }
}
