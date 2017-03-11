module Lib


type HttpRequest = {
  Url : System.Uri
}

type HttpResponse = {
  HttpStatus : int
  ContentType : string
  Response : System.IO.Stream -> unit
}

module Response =
  let text (str:string) = {
     HttpStatus = 200
     ContentType = "text/plain; charset=utf-8"
     Response =
      (fun s ->
        let sw = new System.IO.StreamWriter(s, System.Text.Encoding.UTF8)
        sw.Write(str)
      )
   }


type Test() =
  inherit System.Web.HttpApplication()

  member x.Test = "Hejsan"


type PathPart =
  |Path of string
  |Str

type Route = {
  HttpMethod : string
  Path : PathPart list
}
  with
    override x.ToString() =
      let a = function |PathPart.Path p -> p |PathPart.Str -> sprintf "(string)"
      let c = x.Path |> List.map a |> String.concat "/"
      sprintf "%s - \"/%s\"" x.HttpMethod c

type HttpApp = {
  Routes : unit -> Map<Route,(HttpRequest -> HttpResponse)>
}


let createHttpApp (newroutes) =
  let mutable routes = newroutes
  {
    Routes = (fun () -> routes)
  },
  (fun (k,v) -> routes <- Map.add k v routes  )


module DefaultHttp =
  let private defaultHttpApp = createHttpApp Map.empty
  let app = fst defaultHttpApp
  let extend = snd defaultHttpApp


type HttpApplicationEx() =
  inherit System.Web.HttpApplication()

    member x.Test = "Hej"
