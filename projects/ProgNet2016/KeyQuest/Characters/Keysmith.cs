using KeyQuest.Modules;

namespace KeyQuest.Characters {
    public class Keysmith : NonPlayerCharacter {
        public Keysmith(string name)
            : base(name) {
            IsWorthy = item => item.Name.ToLowerInvariant().Contains("gold");
            Success = " happily sells you a {1} in exchange for your {0}.";
            Failure = " is not running a charity here! If you want {0}, you'll need to give him a {1}.";
            Treasure = new TreasureItem() {
                Name = "Key of Shifting",
                Description = "The Key of Shifting is very mysterious."
            };

        }
    }
}