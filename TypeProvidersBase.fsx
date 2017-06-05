#load "References.fsx"

open FSharp.Data
type QuestionsResult = JsonProvider<"""https://api.stackexchange.com/2.2/questions?site=stackoverflow""">

let getQuestionsByTag tag = 
    QuestionsResult.Load (
        "https://api.stackexchange.com/2.2/search?order=desc&sort=activity&site=stackoverflow&tagged=" + tag)

let dotnetQuestions = getQuestionsByTag ".net"

dotnetQuestions.Items |> Array.iter(fun x -> printf "%s\n" x.Title)
dotnetQuestions.Items |> Array.maxBy (fun x -> x.Score)
dotnetQuestions.Items |> Array.averageBy (fun x -> float x.AnswerCount)