module MartianSymbols

open System
open Network
open NNetTrainer
open NNetSamples



let improveIncrementally guess improve isGoodEnough =
    let rec improveUntilDone state = seq {
        yield state
        if not (isGoodEnough state) then
            let improved = improve state
            yield! improveUntilDone improved }
    guess |> improveUntilDone  



let learn trainingCount learnRate requiredAccuracy = 
    printf "Loading samples..."
    let trainingSamples, testSamples = 
        loadSymbols trainingCount
        |> splitTrainAndTest 0.2
    printfn " done."

    let checkCorrect (target : seq<float>) (output : seq<float>)= 
        let indexOfMaxValue numbers =
            numbers
            |> Seq.mapi(fun i n -> i, n)
            |> Seq.maxBy snd
            |> fst
        indexOfMaxValue target = indexOfMaxValue output

    let calcStats  = calculateStats checkCorrect trainingSamples testSamples

    let newInfo  = 
      { Network = createNetwork 784 [500; 100] 10 
        LearnRate = learnRate
        TrainingStats = {Total = 0; Correct = 0; Percent = nan  }               
        TestStats = {Total = 0; Correct = 0; Percent = nan } } 


    printf "Evaluating untrained network..."
    let startInfo = newInfo |> calcStats 
    printfn " done. "         

    let improve samples net = 
        printfn "Training..."
        shuffleInPlace samples
        trainAllSamples samples net
        printfn "Evaluating..."
        net |> calcStats 

    let doneTraining trainingInfo = trainingInfo.TestStats.Percent > requiredAccuracy

    improveIncrementally 
        startInfo
        (improve trainingSamples)
        doneTraining



