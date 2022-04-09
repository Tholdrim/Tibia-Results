using TibiaResults.Models;

namespace TibiaResults.Interfaces
{
    internal interface IResult
    {
        CategoryResult this[Category category] { get; }

        IEnumerable<Category> Categories { get; }

        void Add(Category category, CategoryResult categoryResult);
    }
}
