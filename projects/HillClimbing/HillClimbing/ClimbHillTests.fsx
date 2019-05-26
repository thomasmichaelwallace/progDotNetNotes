// Simply highlight and pass to FSI (alt+enter or ctrl+enter) then type runTests();; in the FSI window

#load "Support.fs"
#load "Surface.fs"
#load "Landscape.fs"
#load "ClimbHill.fs"

open ProgNet
open ProgNet.ClimbHill

let elevationMap = Landscape.getElevationMap()
let getElevation point = elevationMap point.X point.Y 
let length = 2813.5
let startPoint = 
    { X = 0.5 * length 
      Y = 0.5 * length }
let initialState = 
    { Point = startPoint
      Step = length 
      Elevation = getElevation startPoint }

let ``Freddie will locate 4 point around his location`` () : unit =
    let expectedNeighboursCount = 4
    let neighboursFound = neighbours initialState |> List.length

    assertFirstEqualToSecond 4 neighboursFound "See Task (a) line 49 of ClimbHill.fs"


let ``Calling newState will return an elevation of 2433.703546 (metres)`` () =
    let newState = newState initialState getElevation startPoint
    let expected = 2433.703546
    let actual = newState.Elevation

    AreSame expected actual "See Task (b) line 58 of ClimbHill.fs"

let ``The best candidate position should elevate Freddy to 2847.257974 (metres)`` () =
    let expected = 2847.257974
    let bestCandidate = bestCandidate initialState getElevation
    let bestElevation = bestCandidate.Elevation
    
    AreSame expected bestElevation "See Tasks (c), (d) line 67 of ClimbHill.fs"

let ``Calling findHigherPoint should elevate Freddy to 2847.257974 (metres)`` () =
    let expected = 2847.257974
    let bestCandidate = findHigherPoint initialState getElevation
    let bestElevation = bestCandidate.Elevation
    
    AreSame expected bestElevation "See Task (e) line 79 of ClimbHill.fs"

let ``The mission is complete when Freddies next move would be smaller than the minStep`` () =
    let missionIsCompleted = missionComplete 50.0 { initialState with Step = 49.9 }
    
    isTrue missionIsCompleted "Mission incomplete: See Task (e) line 79 of ClimbHill.fs"

let ``After climbing as high as possible Freddie should be elevated to a point over 3700 (metres)`` () =
    let finalPoint = 
      climb initialState.Point Landscape.length getElevation
      |> Seq.maxBy (fun s -> s.Elevation)

    assertFirstGreaterThanSecond finalPoint.Elevation 3700.0
     
let runTests() =
    [ "Freddie will locate 4 point around his location", ``Freddie will locate 4 point around his location``
      "The best candidate position should elevate Freddy to 2847.257974 (metres)", ``The best candidate position should elevate Freddy to 2847.257974 (metres)``
      "Calling newState will return an elevation of 2433.703546 (metres)", ``Calling newState will return an elevation of 2433.703546 (metres)``
      "Calling findHigherPoint should elevate Freddy to 2847.257974 (metres)", ``Calling findHigherPoint should elevate Freddy to 2847.257974 (metres)``
      "The mission is complete when Freddies next move would be smaller than the minStep", ``The mission is complete when Freddies next move would be smaller than the minStep``
      "After climbing as high as possible Freddie should be elevated to a point over 3700 (metres)", ``After climbing as high as possible Freddie should be elevated to a point over 3700 (metres)``
    ]
    |> Seq.iter ( fun (n,f) -> printfn "Running test: %s" n; f ())


