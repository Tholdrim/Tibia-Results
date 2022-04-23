using System.Text;
using TibiaResults.Interfaces;
using TibiaResults.Models;

namespace TibiaResults.Formatters
{
    internal class DiscordFormatter : IResultFormatter
    {
        public string FormatResult(IResult result)
        {
            var stringBuilder = new StringBuilder();

            foreach (var category in result.Categories.OrderBy(c => c.Order))
            {
                var categoryResult = result[category];
                var formattedCategoryResult = FormatCategoryResult(categoryResult);

                stringBuilder.AppendLine($"**{category.Name}**");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(formattedCategoryResult);
            }

            return stringBuilder.ToString();
        }

        private static string FormatCategoryResult(CategoryResult categoryResult) => categoryResult switch
        {
            { IsAvailable: false } => $":construction: *Not available*{Environment.NewLine}",
            { IsEmpty: true }      => $":open_file_folder: *No entries*{Environment.NewLine}",
            _                      => FormatEntryList(categoryResult.Entries)
        };

        private static string FormatEntryList(IEnumerable<CategoryResultEntry> entries)
        {
            var stringBuilder = new StringBuilder();

            var rankedEntries = entries.Where(e => e.Rank.HasValue).OrderBy(e => e.Rank);
            var unrankedEntries = entries.Where(e => !e.Rank.HasValue).OrderByDescending(e => e.Value);

            foreach (var entry in rankedEntries)
            {
                var formattedProgress = FormatEntryProgress(entry);

                stringBuilder.AppendLine($"{entry.Rank}. {entry.Name} - {entry.Value:N0}{formattedProgress}");
            }

            if (unrankedEntries.Any())
            {
                stringBuilder.AppendLine();
            }

            foreach (var entry in unrankedEntries)
            {
                var formattedProgress = FormatEntryProgress(entry);

                stringBuilder.AppendLine($"{entry.Name} - approximately {entry.Value:N0}{formattedProgress}");
            }

            return stringBuilder.ToString();
        }

        private static string FormatEntryProgress(CategoryResultEntry entry)
        {
            const string formatString = "+###,###,###,###,###;-###,###,###,###,###";

            return entry switch
            {
                { Progress: 0 }                     => string.Empty,
                { Progress: null }                  => " :new:",
                { IsApproximate: true, Rank: { } }  => $" :new: (**approximately {entry.Progress.Value.ToString(formatString)}**)",
                { IsApproximate: true, Rank: null } => $" (**approximately {entry.Progress.Value.ToString(formatString)}**)",
                _                                   => $" (**{entry.Progress.Value.ToString(formatString)}**)"
            };
        }
    }
}
