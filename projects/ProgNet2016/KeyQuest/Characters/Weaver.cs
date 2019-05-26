using System;
using KeyQuest.Modules;

namespace KeyQuest.Characters {
    public class Weaver : NonPlayerCharacter {
        public Weaver(string name)
            : base(name) {
            IsWorthy = item => true;
            Success = " is so happy to see you, he gives you the {1} - for nothing!";
            Failure = String.Empty;
            Treasure = new TreasureItem() {
                Name = "Cloak of Demeter",
                Description = "The cloak of Demeter hides information, as well as brave adventurers"
            };
        }
    }
}