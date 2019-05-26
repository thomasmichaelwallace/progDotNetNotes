// Simply highlight and pass to FSI (alt+enter or ctrl+enter) then type runTests();; in the FSI window

#load "Support.fs"
#load "Surface.fs"
#load "Landscape.fs"
#load "ClimbHill.fs"
#load "Fuel.fs"
#load "BatteryLife.fs"

open ProgNet
open ProgNet.Fuel
open ProgNet.ClimbHill
open ProgNet.Landscape
open ProgNet.BatteryLife


(*
let getBatteryLife () =
    printTitle "Estimating Battery Life"

    let bestLine = BatteryLife.findBestLine Fuel.Readings

    let totalMinutes = bestLine.InitialFuel /  (- bestLine.Slope)
    let missionStart = DateTime(2016, 6, 1, 0, 0, 0)
    let elapsedMinutes = DateTime.Now.Subtract(missionStart).TotalMinutes
    let remainingMinutes = totalMinutes - elapsedMinutes
    printfn "%s minutes remaining." (remainingMinutes.ToString("n1"))
*)

let ``Freddie will calculate the remaining fuel given the testLine to be 635.45A-h`` () : unit =
    let testLine =
        { InitialFuel = 33.7
          Slope = 8.3 }

    let readingTime = 72.5

    let remainingBattery = calcFuel testLine readingTime

    assertFirstEqualToSecond 635.45 remainingBattery "See Task (a) line 18 of BatteryLife.fs"

let ``Freddie will calculate the distance between the estimated line`` () : unit =
    let testLine =
        { InitialFuel = 33.7
          Slope = 8.3 }
    let reading =
        { Time = 72.5
          Value = 35.4 }

    let distance = distanceOfReading testLine reading
    AreSame 360060.0025 distance "See Task (b) line 25 of BatteryLife.fs"

let ``Freddie will calculate the distance between all the readings and the estimated line`` () : unit =
    let testLine =
        { InitialFuel = 3492.00237
          Slope = -0.000000000001 }

    let distance = distanceBetween Readings testLine
    assertFirstEqualToSecond 3039446 (int distance) "See Task (c) line 29 of BatteryLife.fs"

let ``Calculating distance given a point`` () : unit =
    let point =
        { X = 1492.1
          Y = 0.000031 }

    let distance = getDistance Readings point
    assertFirstEqualToSecond 3100761 (int distance) "See Task (d) line 41 of BatteryLife.fs"


 
         
let runTests() =
    [ "Freddie will calculate the remaining fuel given the testLine to be 635.45A-h", ``Freddie will calculate the remaining fuel given the testLine to be 635.45A-h``
      "Freddie will calculate the distance between the estimated line", ``Freddie will calculate the distance between the estimated line``
      "Freddie will calculate the distance between all the readings and the estimated line", ``Freddie will calculate the distance between all the readings and the estimated line``
      "Calculating distance given a point", ``Calculating distance given a point``
    ]
    |> Seq.iter ( fun (n,f) -> printfn "Running test: %s" n; f ())


