module ProgNet.Antenna

let L, R, U, D = 'L', 'R', 'U', 'D'
let Parts = [L; R; U; D;]
let random = System.Random()

(*
let designAntenna () =
    printTitle "Designing Antenna"
    let display design = 
        printf "%s%% - " (design.Reception.ToString("n2"))
        printfn "%s" (design.Parts |> Array.ofList |> String)
//        for (x, y) in Reception.toPoints design.Parts do
//            printfn "%d\t%d" x y
               
    Antenna.design ()
    |> Seq.map Seq.head
    |> Seq.iter display
    //|> Seq.last |> display
*)

type Design =
  { Parts : char list
    Reception : float }

let reception parts = Reception.test parts

//let generateList getMember length =
//    length
//    |>  List.unfold (fun length ->
//            if length = 0 then None
//            else Some (getMember (), length - 1))

// a) Creating a design to test with. Here we indicate how many parts
// that Freddie has at his disposal to create an aerial. Your task,
// create the Design using parts that have been randomly created. 
let createDesign () = 
    let partCount = 128
    let randomPart _ = Parts.[random.Next(Parts.Length)]
    let parts = List.init partCount randomPart
    { Parts = parts
      Reception = reception parts }

let createDesigns designCount = 
    List.init designCount (fun _ -> createDesign () )
       
// b) To get to mission success we need the reception of the communications
// array to achieve a target reception of over 80.0. Using the designs that
// have been generated, check if the first design, in the list of designs,
// exceeds the target reception.
let missionComplete designs =
    let targetReception = 80.0    
    (List.head designs).Reception > targetReception

// c) We will shuffle our lists of designs ready for
// randomly breeding one design with another.
let shuffle : Design list -> Design list = 
    List.sortBy(fun _ -> random.Next())

// d) Now we can pair our designs ready for breeding.
// Use List.zip to pair a couple ready for the breeding.
// You want to zip together the designs to the shuffled 
// set of designs.
let selectPairs designs =
    designs
    |> shuffle
    |> List.zip designs 

// e) Now we need to mate one design with its counterpart
// we take a random split and we then perform a List.take
// using split as the count on design1 then we skip using
// the split for the count on design2. After you have this
// List.concat the two together design1 first, design2 next.
let crossover (design1, design2) =
    let split = random.Next(design1.Parts.Length)
    let newParts = List.concat [ design1.Parts |> Seq.take split |> Seq.toList
                                 design2.Parts |> Seq.skip split |> Seq.toList ]
    { Parts = newParts; Reception = reception newParts }

let createCrossovers designs count =
    designs
    |> selectPairs
    |> Seq.take count
    |> Seq.toList
    |> List.map crossover

let evolve designs =
    let designCount = List.length designs

    let cull designs =
        designs
        |> Seq.mapi (fun i design -> (i, design))
        |> Seq.filter (fun (i, design) -> 
           i < random.Next(designCount) && random.NextDouble() > 0.2) 
        |> Seq.map snd
        |> List.ofSeq

    let survivors = cull designs
    let replacementCount = designCount - survivors.Length
    let crossoverCount = (9 * replacementCount) / 10
    let crossovers = createCrossovers designs crossoverCount
    let newDesigns = createDesigns (replacementCount - crossoverCount)
           
    [survivors; crossovers; newDesigns]
    |> Seq.concat
    |> Seq.sortBy (fun design -> -1.0 * design.Reception)
    |> List.ofSeq

let design () = 
    Common.improveIncrementally 
        (createDesigns 1000)
        evolve 
        missionComplete   
     
    