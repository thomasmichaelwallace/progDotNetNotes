using KeyQuest.Modules;

namespace KeyQuest.Characters {
    public class Earl : NonPlayerCharacter {
        public Earl(string name)
            : base(name) {
            IsWorthy = item => item.Name.ToLowerInvariant().Contains("diamond");
            Success = " takes the {0}! Whilst he's examining it, you grab the {1} and hide it under your cloak.";
            Failure = " is furious with you for wasting his time! He tosses the {0} aside, and banishes you to his deepest dungeons... forever. Next time, bring him the Diamond of Multiple Inheritance";
            Treasure = new TreasureItem() {
                Name = "Delete Key",
                Description = "The Delete Key gives you the power to turn back time."
            };
        }
    }
}