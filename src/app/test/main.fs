open Microsoft.FSharp.Compiler.SourceCodeServices
open Microsoft.FSharp.Compiler.Interactive.Shell


open System
open System.IO
open System.Text

// Intialize output and input streams
let sbOut = new StringBuilder()
let sbErr = new StringBuilder()
let inStream = new StringReader("")
let outStream = new StringWriter(sbOut)
let errStream = new StringWriter(sbErr)

// Build command line arguments & start FSI session
let argv = [| "C:\\fsi.exe" |]
let allArgs = Array.append argv [|"--noninteractive"|]

let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration()
let fsiSession = FsiEvaluationSession.Create(fsiConfig, allArgs, inStream, outStream, errStream)

let getSession fn =

  let a = fn fsiSession

  //printfn "%s" (outStream.ToString())
  //printfn "%s" (errStream.ToString())

  a
let executeScript fullPath =
  let rec code i =
    try System.IO.File.ReadAllText(fullPath)
    with :? System.IO.IOException ->
      if i > 50 then reraise() else
        System.Threading.Thread.Sleep(1)
        code (i+1)
  getSession (fun s -> s.EvalInteractionNonThrowing (code 0))

let lockObj = "lockObj"
let processFile fullPath =
  lock lockObj (fun () ->
    printfn "Evaling %s" fullPath
    let sw = System.Diagnostics.Stopwatch()
    sw.Start()
    match executeScript fullPath with
    |Choice1Of2 (), _ ->
        printfn "%s EVAL" fullPath
    //|Choice1Of2 None, _ -> printfn "No value"
    |Choice2Of2 e, err ->
      printfn "Error %s" (e.ToString())
      err
      |> Array.iter (fun x -> printfn "-> %s" (x.ToString()))
    sw.Stop()
    printfn "Took %ims" sw.ElapsedMilliseconds

    let a = Lib.DefaultHttp.app
    let b = (a.Routes ())
    b |> Map.iter
      (fun k v ->
          printfn "%s" (k.ToString())
      )



    ()
  )
[<EntryPoint>]
let main args =
  use w = new FileSystemWatcher()

  w.Path <- args.[0]
  w.Filter <- "*.fsx"
  printfn "Watching %s %s" w.Path w.Filter
  w.Changed.Add(fun x ->processFile x.FullPath)

  System.IO.Directory.GetFiles(args.[0], "*.fsx")
  |> Array.iter processFile

  w.EnableRaisingEvents <- true


  Console.ReadKey() |> ignore


  0
