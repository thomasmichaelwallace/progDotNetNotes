module Antenna

let L, R, U, D = 'L', 'R', 'U', 'D'
let Parts = [L; R; U; D;]
let random = System.Random()

type Design =
  { Parts : char list
    Reception : float }

let reception parts = Reception.test parts

//let generateList getMember length =
//    length
//    |>  List.unfold (fun length ->
//            if length = 0 then None
//            else Some (getMember (), length - 1))

let createDesign () = 
    let partCount = 128
    let randomPart _ = Parts.[random.Next(Parts.Length)]
    let parts = List.init partCount randomPart
    { Parts = parts; Reception = reception parts }

let createDesigns designCount = 
    List.init designCount (fun _ -> createDesign () )
       
let missionComplete designs =
    let targetReception = 80.0
    match designs with
    | first::_ -> first.Reception > targetReception
    | _ -> failwith "Well, this shouldn't happen"

   
let evolve designs =
    let designCount = List.length designs

    let shuffle = List.sortBy (fun _ -> random.Next())
   
    let selectPairs designs =
        List.zip designs (shuffle designs)

    let crossover (design1, design2) =
        let split = random.Next(design1.Parts.Length)
        let newParts = 
            List.concat  [
                design1.Parts |> List.take split
                design2.Parts |> List.skip split ]
        {Parts = newParts; Reception = reception newParts }

    let createCrossovers designs count =
        designs
        |> selectPairs
        |> List.take count
        |> List.map crossover

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
     
    