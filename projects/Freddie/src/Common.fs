module Common

let improveIncrementally guess improve isGoodEnough =
    let rec improveUntilDone state = seq {
        yield state
        if not (isGoodEnough state) then
            let improved = improve state
            yield! improveUntilDone improved }
    guess |> improveUntilDone  


// Alternate implementation using unfold.
let improveIncrementally2 guess improve isGoodEnough =
    guess
    |> Seq.unfold (fun state ->
        if isGoodEnough state then None
        else Some (state, improve state))
