#load "StackoverflowAPI.fsx"
open StackoverflowAPI
open System

let fsQuestions = getTopQuestionsByTag "F#" 10000

let authors =     
    fsQuestions
    |> Array.filter(fun x -> x.Owner.UserType = "does_not_exist" |> not)
    |> Array.map(fun x -> x.Owner.UserId)
    |> Array.distinct
    |> getUsers
    |> Seq.toArray






let parseCountry (location:string) = 
    let country = 
        location.Split(',')
        |> Array.map (fun x -> x.Trim())
        |> Array.tryLast
    let isState (name:string) = 
        (name.Length = 2 && name |> String.forall Char.IsUpper)
    match country with
    | Some x when x |> isState -> Some "United States"
    | Some x when x <> "" -> country
    | _ -> None

let countries = 
    authors 
    |> Array.map(fun x -> parseCountry (defaultArg x.Location ""))
    |> Array.choose (fun x -> x)
    |> Array.groupBy(fun x -> x)
    |> Array.map(fun (country, array) -> country, array |> Array.length)
    |> Array.sortByDescending snd







open XPlot.GoogleCharts

countries
|> Chart.Geo
|> Chart.WithLabel "Popularity"
