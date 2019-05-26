module ProgNet.Program

open System

open ProgNet.Antenna

let printTitle title =
  printfn ""
  printfn ""
  printfn "%s" title
  printfn "%s" (String('=', title.Length))
  printfn ""

[<EntryPoint>]
let main argv = 


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
    Console.ReadKey() |> ignore
    0