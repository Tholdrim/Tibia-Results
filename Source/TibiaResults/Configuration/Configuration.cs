using TibiaResults.Interfaces;
using TibiaResults.Models;

namespace TibiaResults.Configuration
{
    internal class Configuration : IConfiguration
    {
        public Configuration(ApplicationSettings applicationSettings)
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

        public Uri? BlobContainerUri { get; }

        public string? LocalPath { get; }

        public IEnumerable<string> Characters { get; }

        public (DateOnly From, DateOnly To) Dates { get; }
    }
}
