module Lib


type HttpRequest = {
  Url : System.Uri
}

type HttpResponse = {
  HttpStatus : int
}

module Response =
  let text (str:string) = { HttpStatus = 200 }

type Test() =
  inherit System.Web.HttpApplication()

  member x.Test = "Hejsan"


type PathPart =
  |Path of string
  |Str of string

type Route = {
  HttpMethod : string
  Path : PathPart list
}

type HttpApp = {
  Routes : unit -> (Route*(HttpRequest -> HttpResponse)) list
}


let createHttpApp (newroutes) =
  let mutable routes = newroutes

  {
    Routes = (fun () -> routes)
  },
  (fun x -> routes <- x :: routes)


module DefaultHttp =
  let private defaultHttpApp = createHttpApp List.empty
  let app = fst defaultHttpApp
  let extend = snd defaultHttpApp


type HttpApplicationEx() =
  inherit System.Web.HttpApplication()

    member x.Test = "Hej"
