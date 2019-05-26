module ProgNet.Common
let improveIncrementally guess improve isGoodEnough =
    let rec improveUntilDone state = seq {
        yield state
        if not (isGoodEnough state) then
            let improved = improve state
            yield! improveUntilDone improved }
    guess |> improveUntilDone  
    
   
