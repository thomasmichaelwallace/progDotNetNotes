module NNetTrainer

open System
open Network
open NNetSamples

type Stats = 
  { Total : int
    Correct : int 
    Percent : float }

type TrainingSet = 
  { Network : Network
    LearnRate : float
    TrainingStats : Stats
    TestStats : Stats }

let shuffleInPlace samples =
    let rnd = new Random()
    let sampleCount = Array.length samples
    for i in seq{ 0 .. (sampleCount - 1)} do
        let randomIndex = int (rnd.NextDouble() * float sampleCount)
        let rTemp = samples.[randomIndex]
        samples.[randomIndex] <- samples.[i]
        samples.[i] <- rTemp

let trainAllSamples samples trainingSet =
    let {Network = net; LearnRate = learnRate} = trainingSet
    samples 
    |> Array.iter(fun sample ->
        feedForward net sample.Input |> ignore
        propagateBack learnRate net sample.Target |> ignore )

let calculateStats checkCorrect trainingSamples testSamples trainingSet =
    let net = trainingSet.Network
    let getStats samples =
        let total = samples |> Array.length 
        let correct =
            samples
            |> Seq.filter (fun sample -> 
                feedForward net sample.Input 
                net.OutputLayer 
                    |> getLayerValues
                    |> checkCorrect sample.Target )                
            |> Seq.length
        let percent = 100.0 * (float correct) / (float total)
        {Total = total; Correct = correct; Percent = percent }

    {trainingSet with
        TrainingStats = trainingSamples |> getStats
        TestStats = testSamples |> getStats } 

