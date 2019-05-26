module BatteryLife

open Fuel
open ClimbHill

type Line = 
  { InitialFuel : float
    Slope : float }

let calcFuel line time =
    line.InitialFuel + line.Slope * time

let distanceBetween readings line =  
    let distanceOfReading reading =
        let estimatedFuel = calcFuel line reading.Time
        (reading.Value - estimatedFuel) ** 2.0           
    readings
        |> List.averageBy distanceOfReading
        
let findBestLine readings =    
    let getDistance {X = initialFuel; Y = slope} = 
        let line = 
          { InitialFuel = initialFuel
            Slope = slope  }
        distanceBetween readings line
        
    let getHeight point = -1.0 * getDistance point
        
    let highestPointState = 
        ClimbHill.climb {X = 0.0; Y = 0.0} 1.0 getHeight
        |> Seq.last
    let pointOfLeastDistance = highestPointState.Point
    { InitialFuel = pointOfLeastDistance.X
      Slope = pointOfLeastDistance.Y }
    
//let remainingTime readings currentTime =
//    let line = findBestLine Fuel.Readings
//    let totalTime = line.InitialFuel /  (- line.Slope)
//    totalTime - currentTime

            
        
        