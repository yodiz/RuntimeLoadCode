module HttpApplication


type myViewModel = { A : string }

let myView (a:myViewModel) =
  "" //HttpREsponse

let controller =
  [
    [Path "Test"; Int], (fun httpRequest -> myView { A = "Test" })
  ]


controller
