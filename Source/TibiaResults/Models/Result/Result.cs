using TibiaResults.Interfaces;

namespace TibiaResults.Models
{
    internal class Result : Dictionary<Category, CategoryResult>, IResult
    {
        private Result()
        {
        }

        IEnumerable<Category> IResult.Categories => Keys;

        public static IResult CreateNew() => new Result();
    }
}
