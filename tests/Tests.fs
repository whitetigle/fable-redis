module Tests

open System
open Fable.Core
open Fable.Redis
open Fable.Mocha
open Fable.Core.JsInterop

bluebird.promisifyAll redis

let tests  = 
  testList "Commands" [
    testCaseAsync "SET/GET" <| 
      async {
        let client = redis.createClient() 
        let key = "foo"
        let value = "bar"
        let! set = Async.AwaitPromise (client.SETAsync (key,value))
        let! get = Async.AwaitPromise (client.GETAsync key )
        client.quit()
        Expect.equal value get "should work"
      }

    testCaseAsync "GET unknown" <| 
      async {
        let client = redis.createClient() 
        try 
          let! result = !!client.GETAsync "unknown" 
          client.quit()
          Expect.equal result "blulous" "ok"
        with _ -> 
          client.quit()
          Expect.equal true true "should fail"

      }            
  ]

Mocha.runTests tests |> ignore
