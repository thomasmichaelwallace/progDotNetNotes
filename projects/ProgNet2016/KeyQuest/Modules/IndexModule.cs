using System;
using System.Net;
using System.Threading;
using Nancy;

namespace KeyQuest.Modules {
    public class IndexModule : NancyModule {
        public IndexModule() {
            Get["/"] = _ => View["index"];

            Get["/hello/{name?World}"] = parameters => {
                int delay;
                Int32.TryParse(Request.Query["delay"], out delay);
                if (delay > 0) Thread.Sleep(TimeSpan.FromSeconds(delay));
                return ("Hello, " + parameters["name"]);
            };

            Get["/chuck"] = _ => {
                Thread.Sleep(1000);
                using (var wc = new WebClient()) {
                    return (wc.DownloadString("http://api.icndb.com/jokes/random"));
                }
            };
        }
    }
}