// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
open Fake

let buildDir = "./build/"
let appReferences  =
    !! "src/app/**/*.csproj"
      ++ "src/app/**/*.fsproj"
      ++ "src/**/*.fsproj"

// Targets
Target "Clean" (fun _ ->
  CleanDir buildDir
)


Target "Build" (fun _ ->
  MSBuildDebug buildDir "Build" appReferences
  |> Log "AppBuild-Output: "
)

// Default target
Target "Default" (fun _ ->
  trace "Hello World from FAKE"
)

"Clean"
  ==> "Build"
  ==> "Default"

// start build
RunTargetOrDefault "Default"
