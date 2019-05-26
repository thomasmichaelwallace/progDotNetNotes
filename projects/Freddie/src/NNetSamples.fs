module NNetSamples

open System
open System.IO
open System.Text.RegularExpressions


type Sample = { Input : float[]; Target : float[] }


let getFloatsFromText text =
    Regex.Split(@"[\s,]+", text)
    |> Array.map float

let createSample inputText targetText =
  { Input = getFloatsFromText inputText
    Target = getFloatsFromText targetText }

let splitTrainAndTest testPortion samples =
    let rnd = new System.Random()
    Array.partition (fun _ -> rnd.NextDouble() > testPortion) samples


let (/+) path1 path2 = Path.Combine(path1, path2)
let readSamplesFile name =
    File.ReadLines(__SOURCE_DIRECTORY__ /+ "Samples" /+ name)

let loadSymbols maxSamples = 
    let maxSamples = min maxSamples 21000
    let sampleFromLine (line : string ) =
        let fields = Regex.Split(line, ",")
        let expectedNumber = fields.[0] |> int
       
        let target = 
            [|0..9|] |> Array.map (fun i -> if i = expectedNumber then 1.0 else 0.0 )
        let inputs =  
            fields.[1..] |> Array.map float 
        { Input = inputs; Target = target}

    readSamplesFile "martian-digits.csv"
    |> Seq.skip 1
    |> Seq.take maxSamples
    |> Seq.map sampleFromLine
    |> Array.ofSeq
