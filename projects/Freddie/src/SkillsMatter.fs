open System

open ClimbHill
open BatteryLife
open Antenna
open GateData
open RepairGates

open NNetTrainer
open MartianSymbols

let printTitle title =
    printfn ""
    printfn ""
    printfn "%s" title
    printfn "%s" (String('=', title.Length))
    printfn ""
    

let climbMap () = 
    printTitle "Climbing Hill"

    let printState state =
        let {Point = {X = x; Y = y}} = state
        printfn "%sm (%f, %f) step:%f"
           (state.Height.ToString("n0")) x y state.Step 

    climbMap ()
    |> Seq.iter printState
    //|> Seq.last |> printState

let getBatteryLife () =
    printTitle "Estimating Battery Life"

    let bestLine = BatteryLife.findBestLine Fuel.Readings

    let totalMinutes = bestLine.InitialFuel /  (- bestLine.Slope)
    let missionStart = DateTime(2016, 6, 1, 0, 0, 0)
    let elapsedMinutes = DateTime.Now.Subtract(missionStart).TotalMinutes
    let remainingMinutes = totalMinutes - elapsedMinutes
    printfn "%s minutes remaining." (remainingMinutes.ToString("n1"))
    

let designAntenna () =
    printTitle "Designing Antenna"
    let display design = 
        printf "%s%% - " (design.Reception.ToString("n2"))
        printfn "%s" (design.Parts |> Array.ofList |> String)
//        for (x, y) in Reception.toPoints design.Parts do
//            printfn "%d\t%d" x y
               
    Antenna.design ()
    |> Seq.map Seq.head
    |> Seq.iter display
    //|> Seq.last |> display
    

let repairGates () =
    printTitle "Replace Gates"

    let emulateGate gateName gateCases =        
    
        printfn "Training '%s' Gate." gateName
    
        let trainingData  =
            let b2f bool = if bool then 1.0 else -1.0
            gateCases
            |> List.map(fun case -> 
              { Inputs =  [b2f case.A; b2f case.B ]
                Desired = b2f case.Output })

        let display perceptron  =          
            let success, count = successStats trainingData perceptron
            printfn "%d/%d - Bias:%f  Weigths:%A" success count perceptron.BiasWeight perceptron.InputWeights

        trainPerceptron trainingData
        |> Seq.iter display
        //|> Seq.last |> display

        printfn ""
    
    emulateGate "Or" GateData.orCases
    emulateGate "And" GateData.andCases
    emulateGate "Nand" GateData.nandCases
    //emulateGate "Xor" GateData.xorCases

       
let learnMartian () =
    printTitle "Learning Martian"
    let display trainingSet =
        let printStats title {Total = total; Correct = correct; Percent = percent } =
            Console.WriteLine("{0}: {1:n2}% correct ({2:n0}/{3:n0})", 
                title, percent, correct, total)
        trainingSet.TrainingStats |> printStats "    Training"
        trainingSet.TestStats |> printStats "        Test"
        printfn ""
        

    let sampleCount = 2000        // number of samples to use (max 21,000; aprox 25% used for testing)
    let learnRate = 0.05          // learns faster but reduces final accuracy (e.g.: 0.001 - 0.1)
    let requiredAccuracy =  70.0  // when to stop

    MartianSymbols.learn sampleCount learnRate requiredAccuracy
        |> Seq.iter display    



[<EntryPoint>]
let main argv = 
    climbMap ()
    getBatteryLife ()
    designAntenna ()
    repairGates ()
    learnMartian ()

    printfn ""
    printfn "Done."
    Console.ReadKey() |> ignore
    0