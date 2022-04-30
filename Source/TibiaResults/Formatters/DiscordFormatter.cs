using System.Text;
using TibiaResults.Extensions;
using TibiaResults.Helpers;
using TibiaResults.Interfaces;
using TibiaResults.Models;

namespace TibiaResults.Formatters
{
    internal class DiscordFormatter : IResultFormatter
    {
        public string FormatResult(IResult result)
        {
            var formattedCategoryParts = result.Categories
                .OrderBy(c => c.Order)
                .Select(c => FormatCategoryPart(c, result[c]))
                .ToArray();

            return string.Join(Environment.NewLine, formattedCategoryParts);
        }

        private static string FormatCategoryPart(Category category, CategoryResult categoryResult)
        {
            var stringBuilder = new StringBuilder();
            var formattedCategoryResult = FormatCategoryResult(categoryResult);

            stringBuilder.AppendLine($"**{category.Name}**");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(formattedCategoryResult);

            return stringBuilder.ToString();
        }

        private static string FormatCategoryResult(CategoryResult categoryResult) => categoryResult switch
        {
            { IsAvailable: false } => ":construction: *Not available*",
            { IsEmpty: true }      => ":open_file_folder: *No entries*",
            _                      => string.Join(Environment.NewLine, FormatEntryListLines(categoryResult.Entries))
        };

        private static IEnumerable<string> FormatEntryListLines(IEnumerable<CategoryResultEntry> entries)
        {
            var categoryWinners = CategoryHelper.GetWinners(entries);

            var rankedEntries = entries.Where(e => e.Rank.HasValue).OrderBy(e => e.Rank);
            var unrankedEntries = entries.Where(e => !e.Rank.HasValue).OrderByDescending(e => e.Value);

            foreach (var entry in rankedEntries)
            {
                var formattableProgress = GetFormattableEntryProgress(entry);
                var icons = GetEntryIcons(entry, categoryWinners);

                yield return FormattableString.Invariant($"{entry.Rank}. {entry.Name} - {entry.Value:N0}{formattableProgress}{icons}");
            }

            if (unrankedEntries.Any())
            {
                yield return string.Empty;
            }

            foreach (var entry in unrankedEntries)
            {
                var formattableProgress = GetFormattableEntryProgress(entry);
                var icons = GetEntryIcons(entry, categoryWinners);

                yield return FormattableString.Invariant($"{entry.Name} - approximately {entry.Value:N0}{formattableProgress}{icons}");
            }
        }

        private static string GetEntryIcons(CategoryResultEntry entry, IDictionary<string, int?> categoryWinners)
        {
            categoryWinners.TryGetValue(entry.Name, out var place);

            var placeIcon = place switch
            {
                1 => " :first_place:",
                2 => " :second_place:",
                3 => " :third_place:",
                _ => string.Empty
            };

            var newIcon = entry switch
            {
                { Progress: null }                 => " :new:",
                { IsApproximate: true, Rank: { } } => " :new:",
                _                                  => string.Empty
            };

            return placeIcon + newIcon;
        }

        private static FormattableString GetFormattableEntryProgress(CategoryResultEntry entry) => entry switch
        {
            { Progress: 0 }         => $"",
            { Progress: null }      => $"",
            { IsApproximate: true } => $" (**approximately {entry.Progress.Value.ToFormattableSignedNumber()}**)",
            _                       => $" (**{entry.Progress.Value.ToFormattableSignedNumber()}**)"
        };
    }
}
