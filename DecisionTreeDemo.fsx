#load "references.fsx"
open DecisionTree
open System.IO


let lenses = 
    let file = @"C:\Users\Roman_Nevolin\Documents\Visual Studio 2015\Projects\Demo\Demo\Data\lenses.data"
    let dataset =
        File.ReadAllLines(file)
        |> Array.map (fun line -> line.Split(';'))
    let labels = [| "Age"; "Presc."; "Astigm"; "Tears"; "Decision" |]
    labels, dataset

let lensesTree = build lenses
let subject = [| ("Age", "young"); ("Presc.", "myope"); ("Astigm", "no"); ("Tears", "normal"); |]
let result = classify subject lensesTree