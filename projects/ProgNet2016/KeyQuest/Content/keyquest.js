function report(elementId, message) {
    var element = document.createElement('p');
    var parent = document.getElementById(elementId);
    parent.appendChild(element);
    element.innerHTML = message;
};

function gameOver(message) {
    var element = document.createElement('div');
    element.className = "game-over";
    document.body.appendChild(element);
    element.innerHTML = "<h2>GAME OVER!</h2>" + message;
}

function victory(message) {
    var element = document.createElement('div');
    element.className = "victory";
    document.body.appendChild(element);
    element.innerHTML = "<h2>YOU HAVE WON!</h2>" + message;
}

function questify(url, elementId) {
    return (function (item) {
        var promise = new Promise(function (resolve, reject) {
            var request = new XMLHttpRequest();
            //Send the proper header information along with the request
            request.open('POST', url);
            request.setRequestHeader("Content-type", "application/json");
            request.setRequestHeader("Accept", "application/json");
            request.onload = function () {
                if (request.status == 200) {
                    var result = JSON.parse(request.response);
                    report(elementId, result.text);
                    resolve(result.item); // we got data here, so resolve the Promise
                } else {
                    reject(Error(request.statusText)); // status is not 200 OK, so reject
                }
            };

            request.onerror = function () {
                reject(Error('Error fetching data.')); // error occurred, reject the  Promise
            };
            request.send(JSON.stringify(item)); //send the request
        });
        return (promise);
    });
}

function runQuest() {
    //TODO: extend this runQuest method to complete all three quests and retrieve all three keys.
    const div = "ctrl-div";
    var talkToTheKnight = questify("/knight", div);
    var talkToTheWizard = questify("/wizard", div);
    var talkToTheCleric = questify("/cleric", div);
    var talkToTheWeaver = questify("/weaver", div);
    var talkToTheEarl = questify("/earl", div);
    var talkToTheKeySmith = questify("/keySmith", div);


    var bagOfGold = { name: "Bag of Gold" };
    var things = pickThree(div); // [talkToTheWeaver, talkToTheKnight, talkToTheWizard];



    //things.reduce((p, c) => p.then(c), Promise.resolve(bagOfGold))
    //    .then((r) => console.log(r))
    //    .catch(gameOver);

    discover();
    
    //tryOption(obj, talkToTheWeaver)
        

    //    .then(talkToTheCleric)
    //    .catch(gameOver);
}

function discover() {
    const div = "ctrl-div";
    var bagOfGold = { name: "Bag of Gold" };
    var things = pickThree(div);
    console.log("trying...");
    let to = 0;
    things.reduce((p, c) => p.then((o) => { to += 1; return c(o); }), Promise.resolve(bagOfGold))
        .then((r) => console.log(r))
        .catch(() => { console.log(to); discover(); });
}

function pickCount(div) {
    var talkers = {
        knight: questify("/knight", div),
        wizard: questify("/wizard", div),
        cleric: questify("/cleric", div),
        weaver: questify("/weaver", div),
        earl: questify("/earl", div),
        keySmith: questify("/keySmith", div)
    };
    var opts = ["knight", "wizard", "cleric", "weaver", "earl", "keySmith"]
    for (var i = opts.length - 1; i > 0; i--) {
        var j = Math.floor(Math.random() * (i + 1));
        var temp = opts[i];
        opts[i] = opts[j];
        opts[j] = temp;
    }
    var picks = [opts.pop(), opts.pop(), opts.pop()];
    console.log(picks);
    return [talkers[picks[0]], talkers[picks[1]], talkers[picks[2]]];
}

function solveify(div) {
    var bagOfGold = { name: "Bag of Gold" };



    var known = [];

    tryOption(p, q, r);

};

function tryOption(state, action) {
    state
        .then(action)
}

