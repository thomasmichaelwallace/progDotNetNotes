module GateData

open System

type GateSample = {
    A : bool
    B : bool
    Output : bool }

  
let orCases = 
  [ {A = false; B = false; Output = false }
    {A = false; B = true; Output = true }
    {A = true; B = false; Output = true }
    {A = true; B = true; Output = true } ]    

let andCases = 
  [ {A = false; B = false; Output = false }
    {A = false; B = true; Output = false }
    {A = true; B = false; Output = false }
    {A = true; B = true; Output = true } ]

let nandCases = 
  [ {A = false; B = false; Output = true }
    {A = false; B = true; Output = true }
    {A = true; B = false; Output = true }
    {A = true; B = true; Output = false } ]

let xorCases = 
  [ {A = false; B = false; Output = false }
    {A = false; B = true; Output = true }
    {A = true; B = false; Output = true }
    {A = true; B = true; Output = false } ]
