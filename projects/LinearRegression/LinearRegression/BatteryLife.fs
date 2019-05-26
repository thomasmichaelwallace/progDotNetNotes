module ProgNet.BatteryLife

open Fuel
open ClimbHill

// The Line represents the estimate that Freddie 
// makes regarding the estimated fuel remaining.
type Line =
  { InitialFuel : float
    Slope : float }

// a) To calculate the remaining fuel level 
// we need to multiply the reading at the 
// given time against the slope. When you 
// have that result add it to the initial 
// fuel level and the result is the estimated 
// fuel left.
let calcFuel line reading =
    reading * line.Slope + line.InitialFuel

// b) Pass in each reading one by one and calculate
// the estimated fuel remaining. When the estimated
// fuel level is calculated we can subtract it from
// the reading value then square it. 
let distanceOfReading line reading =
    let estimatedFuel = calcFuel line reading.Time
    (reading.Value - estimatedFuel) ** 2.0

let distanceBetween readings line =
    // c) We need to get the average distance of between
    // all the readings and the line that Freddie is 
    // testing with. We can use List.averageBy on the 
    // readings to get the distance of the reading.    
    readings
    |> List.averageBy (fun r -> distanceOfReading line r)

// d) Get the distance between the readings and
// the estimate that Freddie is making, your job
// is to construct a line from the Point.
let getDistance readings { X = initialFuel; Y = slope } =
    let line =
        { InitialFuel = initialFuel
          Slope = slope }
        // e) calculate the distance between the line and 
        // the and the readings gathered on the ascent.
    distanceBetween readings line

let findBestLine readings =

    // Invert the graph to restrict infinite.
    let getHeight point = -1.0 * getDistance readings point

    // Here we are climbing the hill and gathering the
    // data required, to estimate the remaining fuel.
    let highestPointState =
        ClimbHill.climb { X = 0.0; Y = 0.0 } 1.0 getHeight
        |> Seq.last

    let pointOfLeastDistance = highestPointState.Point

    { InitialFuel = pointOfLeastDistance.X
      Slope = pointOfLeastDistance.Y }

//let remainingTime readings currentTime =
//    let line = findBestLine Fuel.Readings
//    let totalTime = line.InitialFuel /  (- line.Slope)
//    totalTime - currentTime
