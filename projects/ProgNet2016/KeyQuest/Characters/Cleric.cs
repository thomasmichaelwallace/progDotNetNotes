using KeyQuest.Modules;

namespace KeyQuest.Characters {
    public class Cleric : NonPlayerCharacter {
        public Cleric(string name)
            : base(name) {
            IsWorthy = item => item.Name.ToLowerInvariant().Contains("key");
            Success = " gratefully accepts the {0}, and offers you the {1} in exchange.";
            Failure = " is not interested in the {0} - you should have offered her a key. She casts you into a contravariant subclass, and you spend the rest of your life trying to prove that all squares are rectangles.";
            Treasure = new TreasureItem() {
                Name = "Alt Key",
                Description = "The Alt Key gives you the power of shape-shifting."
            };
        }


    }
}