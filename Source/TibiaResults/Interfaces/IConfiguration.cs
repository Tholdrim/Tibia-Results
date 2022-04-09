namespace TibiaResults.Interfaces
{
    internal interface IConfiguration
    {
        Uri? BlobContainerUri { get; }

        string? LocalPath { get; }

        ISet<string> Characters { get; }

        (DateOnly From, DateOnly To) Dates { get; }
    }
}
