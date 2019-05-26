// This code is designed to accommodate complete novices to
// F#, the comments are very verbose to enable those with 
// little or no knowledge of F# to continue with a little issue
// as possible. Apologies to those of you already familiar :-)
// Watch out in the code for segments marked with our special
// type to instruct and guide you as you progress:
//
//     __YOUR_IMPLEMENTION_HERE__
//
// Where you see this type you should read the instructions
// and attempt to add your own implementation there.

module ProgNet.ClimbHill

open System

// Here we use a 'record' type (signified by the curly braces)
// We use it to represent grid position on our landscape.
// Think of it as a coordinate. NB; we could have used
// a 'tuple' type here but we chose a record to avoid confusion 
// with the typical way a method is called in C#.
type Point =
    { X : float 
      Y : float}

// We will use this record to hold information about the locations
// Freddie has visited to survey the altitude and surrounding area.
// Note that we are using the Point type we defined above. All types
// have to be declared before they can be used, unlike C# or VB 
// where you are permitted to define the type after you have made 
// a call to it. It may seem like an unnecessary restriction but it
// really helps the quality of the code you will produce.
type State =
    { Point : Point
      Step : float 
      Elevation: float }

  
// 1) The first task Freddie needs to complete is to know 
// how to discrimate between the various heights of the 
// surrounding terrain. Freddie needs to do this in order
// to be able to move to a higher location.

// stepFactor is the factor for which Freddie will
// gradually reduce to in the search for the 
// necessary altitude to broadcast the message.
let stepFactor = 0.8333

let neighbours current = 
    // a) Get the current point
    let point = current.Point
    let step = current.Step
    [ { point with Y = point.Y - step }
      { point with X = point.X + step }
      { point with Y = point.Y + step }
      { point with X = point.X - step } ]

let newState current getElevation point =
    { current with
        Point = point 
        // b) Get the elevation of the point 
        Elevation = getElevation point }

// c) In F# we use map, which is similar to Select
// in Linq, here you need to use map on neighbours
// and List.map them using the newState function above.
let bestCandidate current getElevation = 
    neighbours current
    |> List.map (newState current getElevation)
    // d) Look on the List type for a function
    // that will help you locate the highest
    // elevation in the data. You will have to
    // use a lambda in the form similar to:
    // (fun point -> point.Elevation)
    |> List.maxBy (fun s -> s.Elevation)

// Here is our first function, we use 'let' to declare a function
// then the name of the function and followed by the arguments.
let findHigherPoint current getElevation =

    let best = bestCandidate current getElevation

    // e) Now you have the bestCandidate you need to
    // compare its elevation to the current elevation.
    // if greater then return that one, otherwise
    // reduce the current step by multiplying it with
    // stepFactor to update the step size.
    if best.Elevation > current.Elevation then
        best
    else
        { current with Step = current.Step * stepFactor }

// f) We know the mission is complete when the 
// current step is less than the minStep.
// Complete this function so Freddie knows
// that the current mission is complete.
let missionComplete minStep state =
    state.Step < minStep

// Now all we have done can be combined.
// If you have put it all together correctly
// then we should be able to run the program
// and Freddie will have the knowledge to
// navigate the landscape and find the 
// necessary altitude to broadcast the message.
let climb startPoint initialStep getHeight  =
    let initialState = 
        { Point = startPoint
          Step = initialStep 
          Elevation = getHeight startPoint }

    let minimumStep = initialStep / 10000.0

    // In F# we can use recursive function
    // simply by adding the 'rec' keyword to
    // the function we create and making sure
    // that the function calls itself in a
    // branch of the logic within the function.
    let rec climbUntilDone state = 
        seq { yield state
              // g) We can recurse only if the mission is 
              // incomplete so Freddie continues searching
              // for location that has sufficient altitude.
              // check if the mission is complete.
              if not (missionComplete minimumStep state) then
                  // h) Now we can locate a place with a higher
                  // altitide using our findHigherPoint, state
                  // and getHeight to get Freddie higher.
                  let improved = findHigherPoint state getHeight

                  // i) Finally we can call the recursive
                  // function with the improved location.
                  // Note we use 'yield!' to take the 
                  // individual items in the seq returned
                  // rather than yielding the seq.
                  yield! climbUntilDone improved }

    // j) Your final task is to kick off the hill climbing;
    // use initialState.
    climbUntilDone initialState


