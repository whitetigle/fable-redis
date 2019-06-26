module Tests.Commands

open System
open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop
open Fable.Core.DynamicExtensions
open Node.OS
open Util

let tests : Test = 
  testList "Commands" [
  
    testList "Test" [
      testCase "Test" <| fun _ ->
        testPassed()
    ]
  ]