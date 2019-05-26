module ClimbHill

open System

type Point =
  { X : float 
    Y : float}

type State =
  { Point : Point
    Step : float 
    Height: float }
 
let findHigherPoint getElevation current =
    let stepFactor = 0.8333

    let neighbours = 
        let point = current.Point
        let step = current.Step
        [ {point with Y = point.Y - step}
          {point with X = point.X + step}
          {point with Y = point.Y + step}
          {point with X = point.X - step} ]

    let newState point =
        { current with
            Point = point 
            Height = getElevation point }

    let bestCandidate = 
        neighbours
        |> List.map newState
        |> List.maxBy (fun s -> s.Height)

    if bestCandidate.Height > current.Height then
      bestCandidate
    else
      { current with Step = current.Step * stepFactor }

let missionComplete minStep state =
    state.Step < minStep

let climb startPoint initialStep getHeight  =
    let initialState = 
      { Point = startPoint
        Step = initialStep
        Height = getHeight startPoint }

    Common.improveIncrementally
        initialState
        (findHigherPoint getHeight)
        (missionComplete (initialStep / 10000.0))

let climbMap () =    
    let mapLength = Landscape.length
 
    let getMapHeight =     
        let elevationMap = Landscape.getElevationMap ()
        (fun point -> elevationMap point.X point.Y)             

    let startPoint = 
        { X = 0.5 * mapLength 
          Y = 0.5 * mapLength }
    
    climb startPoint mapLength getMapHeight
    