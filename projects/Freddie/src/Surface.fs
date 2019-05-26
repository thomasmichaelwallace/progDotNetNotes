module Surface

open System
open System.Text


type Surface = {
    GetElevation : float * float -> float
    Length : float
    MaxElevation : float
    MaxLocation : float * float
    MinElevation : float
    MinLocation : float * float }

           
let nudge (random : Random) range initial =
    let randMinusToPlusOne =
        random.NextDouble() * 2.0 - 1.0
    initial + (range * randMinusToPlusOne)


type Orientation = Inline | Diagonal

// Sets the height of a vertex by referencing the height of its neighbours.
// Orientation determines if it uses the inline neighbours or diagonal neighbours.
let setHeight (heights : float[,]) nudge halfCount orientation vertexToSet  =
    let max = heights.GetLength(0) - 1
    let x, y = vertexToSet 
    let sourcePoints = 
        match orientation with 
        | Inline -> [(x - halfCount, y); (x, y + halfCount); (x + halfCount, y); (x, y - halfCount)]
        | Diagonal -> [(x - halfCount, y - halfCount); (x - halfCount, y + halfCount);
                        (x + halfCount, y + halfCount); (x + halfCount, y - halfCount)]
    heights.[x, y] <-
        sourcePoints
        |> List.filter (fun (x, y) -> x >= 0 && y >= 0 && x <= max && y <= max)
        |> List.map (fun (x, y) -> heights.[x, y])
        |> List.average
        |> nudge    
    
// Creates a 2d array of heights.  
// The length of each dimension is 2^exp.
let createVertexMap random exp =
    let tileCount = pown 2 exp
    let vericiesCount = tileCount + 1
    let heights = Array2D.zeroCreate vericiesCount vericiesCount

    let initialCornerHeight = 0.5
    let nudgeRange = 0.5
    let newCornerHeight () = 
        initialCornerHeight //|> nudge random nudgeRange
    
    // set corner values
    for x in [0; tileCount] do
        for y in [0; tileCount] do
            heights.[x, y] <- newCornerHeight ()

    // enumerate the centre of all squares of given size
    let enumerateCentres halfCount = seq{
        let mids = [halfCount .. (2 * halfCount) .. tileCount]
        for y in mids do
            for x in mids do
                yield (x, y)}

    // enumerate the midpoint of the edges of all squares of given size
    let enumerateMidPoints halfCount = seq{
        let mids = [halfCount .. (2 * halfCount) .. tileCount]
        let edges = [0 .. (2 * halfCount) .. tileCount]
        for y in mids do
            for x in edges do
                yield (x, y)
        for y in edges do
            for x in mids do
                yield (x, y)}

    // populate vertex values for decresaingly small squares
    let rec fillSquares nudgeRange halfCount =
        match halfCount with
        | 0 -> ()
        | _ ->
            let bigNudge = nudge random nudgeRange
            let littleNudge = nudge random (nudgeRange / 2.0)
            enumerateCentres halfCount
            |> Seq.iter (setHeight heights bigNudge halfCount Diagonal)  
            enumerateMidPoints halfCount
            |> Seq.iter (setHeight heights littleNudge halfCount Inline)        
            fillSquares (nudgeRange / 2.0) (halfCount / 2)

    fillSquares (nudgeRange / 2.0) (tileCount / 2)
    heights

// For given float co-ords get the integer co-ords of the square
// that contains them and relative float cords within that square.
let getVertexAndPoint vertexCount point =
    let scale = float (vertexCount - 1)
    let x = scale * fst point
    let y = scale * snd point 
    let vertex = int x, int y
    let subPoint = x - float (int x), y - float (int y)
    vertex, subPoint

// Given the heights of the corners of 'half a square' returns
// the height a point within the triangle.
let heightInTriangle originHeight topHeight rightHeight point =
    let x, y = point
    originHeight
    + x * (rightHeight - originHeight)
    + y * (topHeight - originHeight)


// returns the height at any co-ords
let getHeight (vHeights : float[,]) point =
    // return zero if outside the unit square
    match point with
    | x, _ when x < 0.0 || x >= 1.0 -> 0.0
    | _, y when y < 0.0 || y >= 1.0 -> 0.0
    | x, y ->
    let vertexCount = vHeights.GetLength(0)
    let (vx, vy), (px, py) = getVertexAndPoint vertexCount point
    let bl = vHeights.[vx, vy]
    let tl = vHeights.[vx, vy + 1]
    let tr = vHeights.[vx + 1, vy + 1]
    let br = vHeights.[vx + 1, vy]
    
    // Get height within the bottom right or the
    // top left triangle of the selected square.
    if px >= py then
        heightInTriangle br bl tr (py, (1.0 - px))
    else
        heightInTriangle tl tr bl ((1.0 - py), px)    

let getStats (heights : float[,])  =
    let mutable maxValue = -1.0
    let mutable maxVertex = (-1, -1)
    let mutable minValue = 2.0
    let mutable minVertex = (-1, -1)

    let upper = heights.GetLength(0) - 1    
    for y in [0..upper] do
        for x in [0..upper] do
            let value = heights.[x, y]
            if value > maxValue then
                maxValue <- value
                maxVertex <- (x, y)
            if value < minValue then
                minValue <- value
                minVertex <- (x, y)

    let point (x, y) = float x / float upper, float y / float upper
    maxValue, maxVertex, (point maxVertex), minValue, minVertex, (point minVertex)

let verticiesAsString (heights : float[,]) =
    let sb = new StringBuilder()
    let upper = heights.GetLength(0) - 1
    for y in [0..upper] do
        for x in [0..upper] do
            sb.Append (sprintf "%f " heights.[x, y]) |> ignore
        sb.AppendLine() |> ignore
    sb.ToString()    


let pointsAsString samples surface =
    let sb = new StringBuilder()
    let upper = samples
    let divisor = ((float upper) + 0.1)
    for y in [0..upper] do
        for x in [0..upper] do
            let X = surface.Length * (float x) / divisor
            let Y = surface.Length * (float y) / divisor
            sb.Append (sprintf "%f " (surface.GetElevation (X, Y))) |> ignore
        sb.AppendLine() |> ignore
    sb.ToString()

let getSurface exp random =
    let vertexHeights = createVertexMap random exp
    //printfn "%s" (verticiesAsString vertexHeights)
    let maxValue, _, maxPoint, minValue, _, minPoint = getStats vertexHeights
    {
        GetElevation = getHeight vertexHeights
        Length = 1.0
        MaxElevation = maxValue
        MaxLocation = maxPoint
        MinElevation = minValue
        MinLocation = minPoint }
    
let scale length minElevation maxElevation surface =
    let scaleInput (x, y) = surface.Length * x/length, surface.Length * y/length 
    let scaleOutput output = 
         output * (maxElevation - minElevation) / (surface.MaxElevation - surface.MinElevation)
    let rawOutput point = surface.GetElevation (scaleInput point)
    let getElevation point =
        minElevation + (scaleOutput ((rawOutput point) - surface.MinElevation))
    let scaleLocation (x, y) = x * length / surface.Length, y * length / surface.Length
    { 
        GetElevation = getElevation
        Length = length
        MaxElevation = maxElevation
        MaxLocation = scaleLocation surface.MaxLocation
        MinElevation = minElevation 
        MinLocation = scaleLocation surface.MinLocation }
    