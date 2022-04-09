using TibiaResults.Models;

namespace TibiaResults.Interfaces
{
    internal interface ILevelTracker
    {
        Levels GetCharacterLevels(string character);

        void Update(string character, Levels characterLevels);
    }
}
