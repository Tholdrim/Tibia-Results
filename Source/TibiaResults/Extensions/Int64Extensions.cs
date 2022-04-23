namespace TibiaResults.Extensions
{
    internal static class Int64Extensions
    {
        public static FormattableString ToFormattableSignedNumber(this long value) => $"{value:+###,###,###,###,##0;-###,###,###,###,###}";
    }
}
