namespace TibiaResults.Interfaces
{
    internal interface IConfigurationService
    {
        Uri? BlobContainerUri { get; }

        string? LocalPath { get; }

        IEnumerable<string> Characters { get; }

        (DateOnly From, DateOnly To) Dates { get; }
    }
}
