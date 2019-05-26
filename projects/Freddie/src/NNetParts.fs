module NNetParts

open System


//// Activation ////

type Activation = 
   {CalcValue : float -> float
    CalcDerivative : float -> float
    GetRandomWeight : unit -> float}

let rnd = new Random()
let public sigmoidActivation = 
   {CalcValue = (fun x -> 1. / (1. + exp -x))
    CalcDerivative = (fun x -> x * (1. - x))
    GetRandomWeight = (fun () -> rnd.NextDouble() * 2. - 1.)} 


//// Neuron ////

type Neuron = 
   {InboundWeights : float[]
    mutable Value : float
    mutable Error : float}

let createNeuron inboundConnectionCount getRandomWeight = 
   {InboundWeights = 
        seq{1..inboundConnectionCount} 
        |> Seq.map (fun _ -> getRandomWeight())
        |> Seq.toArray
    Value = 0.
    Error = 0.}


//// Layer ////

type Layer = 
   {Neurons : Neuron[]}

let createLayer neuronCount inboundConnectionCount getRandomWeight = 
   {Neurons = 
        seq{ 1 .. neuronCount }
        |> Seq.map (fun _ -> createNeuron inboundConnectionCount getRandomWeight)
        |> Seq.toArray}