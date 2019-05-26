using KeyQuest.Modules;

namespace KeyQuest.Characters {
    public class Knight : NonPlayerCharacter {
        public Knight(string name)
            : base(name) {
            // The knight is only interested in swords.
            IsWorthy = item => item.Name.ToLowerInvariant().Contains("cloak of demeter");
            Success = " does not see you, thanks to {0}, and you quietly sneak away with {1}.";
            Failure = " strikes you down with his sword. Next time, obtain the Cloak of Demeter from /weaver first so he doesn't see you.";
            Treasure = new TreasureItem() {
                Name = "Potion of Immutability",
                Description = "This potion provides powerful defence against side-effects."
            };
        }
    }
}