module ProgNet.Program

open System

open ProgNet.ClimbHill

[<EntryPoint>]
let main argv = 
    let printTitle title =
        printfn ""
        printfn ""
        printfn "%s" title
        printfn "%s" (String('=', title.Length))
        printfn ""

    printTitle "Estimating Battery Life"

    let bestLine = BatteryLife.findBestLine Fuel.Readings

    let totalMinutes = bestLine.InitialFuel /  (- bestLine.Slope)
    let missionStart = DateTime(2016, 6, 1, 0, 0, 0)
    let elapsedMinutes = DateTime.Now.Subtract(missionStart).TotalMinutes
    let remainingMinutes = totalMinutes - elapsedMinutes
    printfn "%s minutes remaining." (remainingMinutes.ToString("n1"))

    Console.ReadKey() |> ignore
    0
