module Png

open System
open System.Drawing
open System.IO

let walk design = 
    let addPart endPoint part =
        let h, v = endPoint
        match part with
        | 'L' -> h - 1, v
        | 'R' -> h + 1, v
        | 'U' -> h, v + 1
        | 'D' -> h, v - 1
        | _ -> failwith "Unexpected character. Expected L, R, U or D"
    design 
    |> fun (design : string) -> design.ToCharArray()
    |> List.ofArray
    |> List.scan addPart (0, 0)

let getBounds coords =
    ((0, 0, 0, 0), coords)
    ||> List.fold (fun (hMin, hMax, vMin, vMax) (h, v) ->              
        (min hMin h, max hMax h, min vMin v, max vMax v))

let shift coords xShift yShift  =
    coords
    |> List.map (fun (x, y) -> (x + xShift, y + yShift))

let coordInfo design =
    let coords = walk design
    let (minX, maxX, minY, maxY) = coords |> getBounds
    (shift coords -minX -minY), maxX - minX, maxY - minY

let drawLine (bm : Bitmap) color a b = 
    let (x1, y1), (x2, y2) = a, b
    let xDiff, yDiff = (x2 - x1), (y2 - y1)
    let steps = max (abs xDiff) (abs yDiff)
    for step in [0..steps] do
        let x = x1 + (xDiff * step) / steps
        let y = y1 + (yDiff * step) / steps
        bm.SetPixel(x, y, color)
        

let drawDesign  (bm : Bitmap) scale color coords=
    coords 
    |> Seq.map (fun (x, y) -> (x * scale, y * scale))
    |> Seq.pairwise
    |> Seq.iteri(fun i (a, b) ->
        drawLine bm color a b)

let filename = "design.png"
let saveImage designA designB=
    let dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    let imagePath = Path.Combine(dir, filename)

    let [aCoords, aXMax, aYMax; bCoords, bXMax, bYMax] = 
        [designA; designB] 
        |> List.map coordInfo

    let xMax = max aXMax bXMax
    let yMax = max aYMax bYMax

    let scale = 40
    let bm = new Bitmap(xMax * scale + 1, yMax * scale + 1)
    drawDesign bm scale (Color.Red) aCoords
    drawDesign bm scale (Color.Green) bCoords
    bm.Save(imagePath)

    
   