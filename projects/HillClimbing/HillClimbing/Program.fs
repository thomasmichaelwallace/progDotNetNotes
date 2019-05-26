module ProgNet.Program

open System

open ProgNet.ClimbHill

// Can you describe this John?
let euler x y = Math.E ** -(x ** 2.0 + y ** 2.0)

let printState state =
    let { Point = { X = x; Y = y } } = state
    printfn "%sm (%f, %f) step: %f" 
        (state.Elevation.ToString("n0")) x y state.Step  

[<EntryPoint>]
let main argv = 

    let length = Landscape.length
    let elevationMap = Landscape.getElevationMap ()
    let getHeight point = elevationMap point.X point.Y             
    
    let startPoint = 
        { X = 0.5 * length 
          Y = 0.5 * length }

    climb startPoint length getHeight
    |> Seq.iter printState

    Console.ReadKey() |> ignore
    0