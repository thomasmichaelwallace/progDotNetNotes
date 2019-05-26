using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using KeyQuest.Characters;
using Nancy;
using Nancy.ModelBinding;
using Nancy.ViewEngines.Razor;

namespace KeyQuest.Modules {
    public class QuestModule : NancyModule {

        public object Encounter(NonPlayerCharacter npc) {
            try {
                var treasure = this.Bind<TreasureItem>();
                return (npc.Interact(treasure));
            } catch (Exception ex) {
                return new Response {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = ex.Message
                };
            }
        }

        private readonly Knight knight = new Knight("Sir Timber Nurzly");
        private readonly Cleric cleric = new Cleric("Baliskov");
        private readonly Weaver weaver = new Weaver("The Dream-Weaver");
        private readonly Earl earl = new Earl("Earl Ang");
        private readonly Wizard wizard = new Wizard("Ballmerack");
        private readonly Keysmith keysmith = new Keysmith("Cherry");
        private Dictionary<string, NonPlayerCharacter> characters;
        public QuestModule() {

            characters = new Dictionary<string, NonPlayerCharacter> {
                {"/knight", knight},
                {"/cleric", cleric},
                {"/weaver", weaver},
                {"/earl", earl},
                {"/wizard", wizard},
                {"/keysmith", keysmith}
            };
            foreach (var path in characters.Keys) {
                Post[path] = _ => Encounter(characters[path]);
            }

            Get["/help"] = _ => {
                return View["help", characters];
            };
            //Post["/knight"] = _ => Encounter(knight);
            //Post["/cleric"] = _ => Encounter(cleric);
            //Post["/earl"] = _ => Encounter(earl);
            //Post["/wizard"] = _ => Encounter(wizard);
            //Post["/weaver"] = _ => Encounter(weaver);
            //Post["/keysmith"] = _ => Encounter(keysmith);

        }
    }
}