using KeyQuest.Modules;

namespace KeyQuest.Characters {
    public class Wizard : NonPlayerCharacter {
        public Wizard(string name)
            : base(name) {
            IsWorthy = item => item.Name.ToLowerInvariant().Contains("potion");
            Success = " takes the {0}! Whilst he's examining it, you hide the {1} under your jerkin.";
            Failure = " is not interested in {0}. You should have brought him a potion instead.";
            Treasure = new TreasureItem() {
                Name = "Control Key",
                Description = "The Control Key gives you the power to control animals."
            };
        }
    }
}