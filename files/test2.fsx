#r "../build/Lib.dll"

let a = 5

type MyViewModel = { A : string }

let myView (a:string) =
  Lib.Response.text "Test"

let controller =
  [
    ("POST", []), (fun (httpRequest:int) -> myView "")
  ]


//SetRoute controller

open Lib

let route =  { Route.HttpMethod = "GET"; Route.Path = [] }

DefaultHttp.extend (route, (fun _ -> Response.text "Micke är bäst "))
