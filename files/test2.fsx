let a = 5

type MyViewModel = { A : string }

let myView (a:string) =
  "" //HttpREsponse

let controller =
  [
    ("POST", []), (fun (httpRequest:int) -> myView "")
  ]


//SetRoute controller
