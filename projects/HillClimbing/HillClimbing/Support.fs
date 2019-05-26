[<AutoOpen>]
module ProgNet.Support

let __YOUR_IMPLEMENTION_HERE__<'T> : 'T = raise <| new System.NotImplementedException("You must implement this to continue") 

let isTrue check msg =
    if check
    then printfn "Test passed."
    else failwith (sprintf "Test failed! %s" msg)

let assertFirstEqualToSecond x y msg = 
    if x = y
    then printfn "Test passed."
    else failwith (sprintf "Test failed! %A is not equal to %A: %s" x y msg)
     
let assertFirstGreaterThanSecond x y = 
    if not (x > y)
    then failwith (sprintf "Test failed! %A is not greater than %A" x y)
    else printfn "Test passed."

let assertFirstLessThanSecond x y = 
    if not (x < y) 
    then failwith (sprintf "Test failed! %A is not less than %A" x y)
    else printfn "Test passed."

let AreSame (x:float) y msg =
    let diff = abs (x - y)

    if diff < 0.00001  
    then printfn "Test passed."
    else failwith  (sprintf "Test failed! %A is equivalent %A: %A" x y msg)

let AreCloseEnough (x:float) y msg =
    let diff = abs (x - y)

    if diff < 0.000000000001  
    then printfn "Test passed."
    else failwith  (sprintf "Test failed! %A is equivalent %A: %A" x y msg)
