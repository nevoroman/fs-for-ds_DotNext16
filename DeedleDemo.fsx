#load "references.fsx"

open Deedle
open FSharp.Data
open XPlot.GoogleCharts
open XPlot.GoogleCharts.Deedle


type WorldData = XmlProvider<"http://api.worldbank.org/countries/indicators/NY.GDP.PCAP.CD?date=2010:2010">
let wb = WorldBankData.GetDataContext()
let inds = wb.Countries.World.Indicators
let indUrl = "http://api.worldbank.org/countries/indicators/"

let getData year indicator =
  let query =
    [ ("per_page","1000"); 
      ("date",sprintf "%d:%d" year year) ]
  let data = Http.RequestString(indUrl + indicator, query)
  let xml = WorldData.Parse(data)
  let orNaN value = 
    defaultArg (Option.map float value) nan
  series [ for d in xml.Datas ->
             d.Country.Value, orNaN d.Value ]




let codes =
 [ "CO2", inds.``CO2 emissions (metric tons per capita)``
   "Univ", inds.``School enrollment, primary (gross), gender parity index (GPI)``
   "Life", inds.``Life expectancy at birth, total (years)``
   "Growth", inds.``GDP per capita growth (annual %)``
   "Pop", inds.``Population growth (annual %)``
   "GDP", inds.``GDP per capita (current US$)`` ]

let world = 
  frame [ for name, ind in codes -> 
            name, getData 2013 ind.IndicatorCode ]

let co2 = world?CO2

series [
  "Mean" => Stats.mean co2
  "Max" => Option.get (Stats.max co2)
  "Min" => Option.get (Stats.min co2)
  "Median" => Stats.median co2 ]

frame [
  "Mean" => Stats.mean world
  "Max" => Stats.max world
  "Min" => Stats.min world
  "Median" => Stats.median world ]

