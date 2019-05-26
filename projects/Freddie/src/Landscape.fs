module Landscape

open Surface

open System
open System.IO
open System.Text

let length = Math.PI * 2229.0

let getElevationMap () =

    let random seed =
        let seed =
            match seed with
            | None -> Random().Next()
            | Some seed -> seed
        //printfn "Seed: %d" seed
        Random(seed)

//    let writeFile name text =
//        let dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
//        let file = Path.Combine(dir, name)
//        File.WriteAllText(file, text)
//
//    let printSummary s = 
//        printfn "Max: %f @ %A; Min: %f %A" s.MaxElevation s.MaxLocation s.MinElevation s.MinLocation

    let getLandscape exp length min max seed  =
        random seed    
        |> getSurface exp     
        |> scale length min max 

//    let rec repeat () =
//        getSurface 10 (random None) 
//        |> printSummary
//        //Console.ReadKey() |> ignore
//        repeat()
//    repeat ()

    let seed = Some 751071074 
    let landscape = getLandscape 10 length 545.0 3848.0 seed

//    landscape
//        |> pointsAsString 254
//        |> writeFile "points.txt"

    fun x y -> landscape.GetElevation (x, y)