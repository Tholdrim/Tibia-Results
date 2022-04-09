using TibiaResults.Interfaces;

namespace TibiaResults.Models
{
    internal class LevelTracker : Dictionary<string, Levels>, ILevelTracker
    {
        private LevelTracker()
        {
        }

        public Levels GetCharacterLevels(string character)
        {
            if (TryGetValue(character, out var characterLevels))
            {
                return characterLevels;
            }

            return new(null, null);
        }

        public void Update(string character, Levels characterLevels)
        {
            if (characterLevels is (null, null))
            {
                return;
            }

            this[character] = characterLevels;
        }

        public static ILevelTracker CreateEmpty() => new LevelTracker();
    }
}
