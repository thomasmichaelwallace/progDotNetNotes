module AST =
   type name = string
   type param = string
   type arg = Number of int | Arg of param
   type command =
      | Forward of arg
      | Left of arg
      | Right of arg
      | SetRandomPosition
      | Repeat of arg * command list
      | Call of name * arg list
      | Proc of name * param list * command list
      | SetRandomColour

open AST
open System

let getVar = 
   let varId = ref 0
   fun () -> incr varId; sprintf "_%d" !varId 

let rec emitCommand command =
      match command with
      | Forward arg -> sprintf "forward(%s);" (emitArg arg)  
      | Left arg -> sprintf "turn(-(%s));" (emitArg arg)
      | Right arg -> sprintf "turn(%s);" (emitArg arg)
      | SetRandomPosition -> "set_random_position();"
      | Call(name,args) ->
         let args = String.Join(",", args |> Seq.map emitArg)
         name+"("+args+");"
      | Proc(name,ps,block) ->
         let ps = String.Join(",", ps)
         sprintf "function %s(%s) { %s }\r\n" name ps (emitBlock block)
      | Repeat(n,commands) ->
         let block = emitBlock commands
         String.Format(
            "for({0}=0;{0}<{1};{0}++) {{\r\n (() => {{{2}}})()\r\n}}", 
               getVar(), emitArg n, block);
      | SetRandomColour -> sprintf "set_random_colour();"
and emitArg arg =
   match arg with
   | Number n -> sprintf "%d" n
   | Arg s -> s
and emitBlock commands =
   String.concat "" [|for command in commands -> emitCommand(command)|]   

let program = 
   [
      Proc("circle",[],
         [Repeat(Number 36,[Forward(Number 2);Right(Number 10)])])
      Repeat(Number 50,[SetRandomColour;SetRandomPosition;Call("circle",[])])
   ]
let generatedJS = emitBlock program

let html = 
   sprintf """<html>
<body>
<canvas id="myCanvas" width="400" height="400"
style="border:1px solid #000000;">
</canvas>
<script>
var c = document.getElementById("myCanvas");
var ctx = c.getContext("2d");
var width = 400;
var height = 400;
var x = width/2;
var y = height/2;
ctx.moveTo(x,y);
var a = 23.0;

function forward(n) {
  x += Math.cos((a*Math.PI)/180.0) * n;
  y += Math.sin((a*Math.PI)/180.0) * n;
  ctx.lineTo(x,y);
  ctx.stroke();
}
function turn(n) {
  a += n;
}

// set-random-position - Note: need to escape minus sign
function set_random_position() {
  x = Math.random() * width;
  y = Math.random() * height; 
  ctx.moveTo(x,y); 
}

// set-to-a-colour
function set_random_colour() {
    let color = "#"+((1<<24)*Math.random()|0).toString(16);
    ctx.strokeStyle=color;
    ctx.beginPath();
}

// Generated JS
%s

</script>
</body>
</html>""" generatedJS

open System.IO

let path = Path.Combine(__SOURCE_DIRECTORY__, "TurtleGen.html")
File.WriteAllText(path, html)
path |> System.Diagnostics.Process.Start