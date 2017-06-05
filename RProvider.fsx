#load "references.fsx"

open RDotNet
open RProvider
open RProvider.graphics
open RProvider.stats
open RProvider.tseries
open RProvider.zoo

open System
open System.Net
open FSharp.Data

type Stocks = CsvProvider<"http://ichart.yahoo.com/table.csv?s=MFST">
 
/// Returns prices of a given stock for a specified number 
/// of days (starting from the most recent)
let getStockPrices stock count =
  let url = "http://ichart.finance.yahoo.com/table.csv?s="
  [| for r in Stocks.Load(url + stock).Take(count).Rows -> float r.Open |] 
  |> Array.rev

/// Get opening prices for MSFT for the last 255 days
let msftOpens = getStockPrices "MSFT" 255

let msft = msftOpens |> R.log |> R.diff

let tickers = 
  [ "MSFT"; "AAPL"; "X"; "VXX"; "GLD" ]
let data =
  [ for t in tickers -> 
      t, getStockPrices t 255 |> R.log |> R.diff ]

let df = R.data_frame(namedParams data)
R.pairs(df)

let widgets = [ 3; 8; 12; 15; 19; 18; 18; 20; ]
let sprockets = [ 5; 4; 6; 7; 12; 9; 5; 6; ]

R.plot(widgets)

R.plot(widgets, sprockets)