module Tests

open System
open Fable.Core
open Fable.Redis
open Fable.Mocha
open Fable.Core.JsInterop

bluebird.promisifyAll redis

let await = Async.AwaitPromise

let tests  = 
  testList "Commands" [
    testCaseAsync "SET/GET" <| 
      async {
        let client = redis.createClient() 
        let key = "foo"
        let value = "bar"
        let! set = await (client.SETAsync (key,value))
        let! get = await (client.GETAsync key )
        client.quit()
        Expect.equal value get "should work"
      }

    testCaseAsync "DEL ONE" <| 
      async {
        let client = redis.createClient() 
        let key = "foo"
        let! del = await (client.DELAsync key )
        client.quit()
        Expect.equal 1 del "should work"
      }

    testCaseAsync "SET/DEL MANY" <| 
      async {
        let client = redis.createClient() 
        let key = "foo"
        let value = "bar"
        let setters =
          [0..3] 
          |> Seq.map( fun i -> client.SETAsync ((sprintf "%s%i" key i),(sprintf "%s%i" value i)))
          |> Promise.Parallel
        let! _ = await setters
        
        let deleters = 
          [0..3] 
          |> Seq.map( fun i -> sprintf "%s%i" key i) 
          |> Seq.toArray
        
        let! delete  = await (client.DELAsync deleters)
        client.quit()
        Expect.equal 4 delete "should work"
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

    testCaseAsync "APPEND NON EXISTING" <| 
      async {
        let client = redis.createClient() 
        try 
          let value = "whatever"
          let! result = await (client.APPENDAsync("unknown",value ))
          client.quit()
          Expect.equal result value.Length "ok"
        with _ -> 
          client.quit()
          Expect.equal true true "should fail"
      }      

    testCaseAsync "APPEND" <| 
      async {
        let client = redis.createClient() 
        try 
          let key = "unknown"
          let value = "whatever"
          let! _ = await (client.SETAsync(key,value ))
          let! result = await (client.APPENDAsync(key,value ))
          let! _ = await (client.DELAsync key)
          client.quit()
          Expect.equal result (value.Length * 2) "ok"
        with _ -> 
          client.quit()
          Expect.equal true false "should fail"
      }      

    testCaseAsync "BITCOUNT" <| 
      async {
        let client = redis.createClient() 
        try 
          let key = "unknown"
          let value = "foobar"
          let! _ = await (client.SETAsync(key,value ))
          let! result = await (client.BITCOUNTAsync key)
          let! del = await (client.DELAsync key)
          client.quit()
          Expect.equal result 26 "ok"
        with _ -> 
          client.quit()
          Expect.equal true false "should fail"
      }      

    testCaseAsync "BITCOUNT with range" <| 
      async {
        let client = redis.createClient() 
        try 
          let key = "unknown"
          let value = "foobar"
          let! _ = await (client.SETAsync(key,value ))
          let! result = await (client.BITCOUNTAsync(key,0,0))
          let! del = await (client.DELAsync key)
          client.quit()
          Expect.equal result 4 "ok"
        with _ -> 
          client.quit()
          Expect.equal true false "should fail"
      }

    testCaseAsync "BITPOS with range" <| 
      async {
        let client = redis.createClient() 
        try 
          let key = "mykey"
          let value = "\x00\xff\xf0"
          let! _ = await (client.SETAsync(key,value ))
          let! result = await (client.BITPOSAsync(key,1,2))
          let! del = await (client.DELAsync key)
          client.quit()
          Expect.equal result 16 "ok"
        with _ -> 
          client.quit()
          Expect.equal true false "should fail"
      }

    testCaseAsync "DECR" <| 
      async {
        let client = redis.createClient() 
        try 
          let key = "mykey"
          let value = "10"
          let! _ = await (client.SETAsync(key,value ))
          let! result = await (client.DECRAsync key)
          let! del = await (client.DELAsync key)
          client.quit()
          Expect.equal result 9 "ok"
        with _ -> 
          client.quit()
          Expect.equal true false "should fail"
      }

    testCaseAsync "DECRBY" <| 
      async {
        let client = redis.createClient() 
        try 
          let key = "mykey"
          let value = "10"
          let! _ = await (client.SETAsync(key,value ))
          let! result = await (client.DECRBYAsync(key,2))
          let! del = await (client.DELAsync key)
          client.quit()
          Expect.equal result 8 "ok"
        with _ -> 
          client.quit()
          Expect.equal true false "should fail"
      }

    testCaseAsync "SETBIT/GETBIT" <| 
      async {
        let client = redis.createClient() 
        try 
          let key = "mykey"
          let! _ = await (client.SETBITAsync(key,7,1))
          let! result = await (client.GETBITAsync(key,7))
          let! _ = await (client.DELAsync key)
          client.quit()
          Expect.equal result 1 "ok"
        with _ -> 
          client.quit()
          Expect.equal true false "should fail"
      }

    testCaseAsync "GETRANGE" <| 
      async {
        let client = redis.createClient() 
        try 
          let key = "mykey"
          let value = "This is a string"
          let! _ = await (client.SETAsync(key,value ))
          let! result = await (client.GETRANGEAsync(key,-3,-1))
          let! _ = await (client.DELAsync key)
          client.quit()
          Expect.equal result "ing" "ok"
        with _ -> 
          client.quit()
          Expect.equal true false "should fail"
      }

    testCaseAsync "GETSET" <| 
      async {
        let client = redis.createClient() 
        try 
          let key = "mykey"
          let value = "Hello"
          let! _ = await (client.SETAsync(key,value ))
          let! result = await (client.GETSETAsync(key,"world"))
          let! _ = await (client.DELAsync key)
          client.quit()
          Expect.equal result value "ok"
        with _ -> 
          client.quit()
          Expect.equal true false "should fail"
      }

    testCaseAsync "INCR/INCRBY" <| 
      async {
        let client = redis.createClient() 
        try 
          let key = "mykey"
          let value = "10"
          let! _ = await (client.SETAsync(key,value ))
          let! _ = await (client.INCRAsync(key))
          let! sresult = await (client.INCRBYAsync(key,3))
          let! del = await (client.DELAsync key)
          client.quit()
          Expect.equal sresult 14 "ok"
        with _ -> 
          client.quit()
          Expect.equal true false "should fail"
      }

    testCaseAsync "INCRBYFLOAT" <| 
      async {
        let client = redis.createClient() 
        try 
          let key = "test"
          let value = "10.50"
          let! _ = await (client.SETAsync(key,value ))
          let! sresult = await (client.INCRBYFLOATAsync(key,0.1))
          let! _ = await (client.DELAsync key)
          client.quit()
          let result = sresult - 10.6 = 0.
          Expect.isTrue result "ok"
        with e -> 
          client.quit()
          Expect.equal true false "should fail"
      }

    testCaseAsync "MGET" <| 
      async {
        let client = redis.createClient() 
        let key = "foo"
        let value = "bar"
        let setters =
          [0..3] 
          |> Seq.map( fun i -> client.SETAsync ((sprintf "%s%i" key i),(sprintf "%s%i" value i)))
          |> Promise.Parallel
        let! _ = await setters
        
        let keys = 
          [0..3] 
          |> Seq.map( fun i -> sprintf "%s%i" key i) 
          |> Seq.toArray
        
        let! mget  = await (client.MGETAsync keys)
        let! _  = await (client.DELAsync keys)
        client.quit()
        Expect.equal 4 mget.Length "should work"
      }      

    testCaseAsync "MSET" <| 
      async {
        let client = redis.createClient() 
        let key = "foo"
        let value = "bar"        
        let keys = 
          [0..3] 
          |> Seq.map( fun i -> sprintf "%s%i" key i) 
          |> Seq.toArray
        
        let data = 
          keys 
          |> Seq.mapi( fun i k -> [k; sprintf "%s%i" value i])
          |> Seq.concat
          |> Seq.toArray

        let! _  = await (client.MSETAsync data)
        let! get  = await (client.GETAsync "foo2")
        let! _  = await (client.DELAsync keys)
        client.quit()
        Expect.equal "bar2" get "should work"
      }


    testCaseAsync "MSETNX" <| 
      async {
        let client = redis.createClient() 
        let key = "foo"
        let value = "bar"        
        let keys = 
          [0..3] 
          |> Seq.map( fun i -> sprintf "%s%i" key i) 
          |> Seq.toArray
        
        let data = 
          keys 
          |> Seq.mapi( fun i k -> [k; sprintf "%s%i" value i])
          |> Seq.concat
          |> Seq.toArray

        let overwritten_data = 
          keys 
          |> Seq.mapi( fun i k -> [k; sprintf "%s%icoco" value i])
          |> Seq.concat
          |> Seq.toArray

        let! _  = await (client.MSETNXAsync data)
        let! _  = await (client.MSETNXAsync overwritten_data)
        let! get  = await (client.GETAsync "foo2")
        let! _  = await (client.DELAsync keys)
        client.quit()
        Expect.equal "bar2" get "should work"
      }

    testCaseAsync "PSETEX/PTTL" <| 
      async {
        let client = redis.createClient() 
        let key = "mykeykey"
        let value = "Hello"
        let expiration = 1000
        let! _ = await (client.PSETEXAsync(key,expiration,value ))
        let! result = await (client.PTTLAsync(key))
        let result = result <= expiration
        client.quit()
        Expect.isTrue result "ok"
      }

    testCaseAsync "PSETEX" <| 
      async {
        let client = redis.createClient() 
        try 
          let key = "mykey"
          let value = "Hello"
          let expiration = 100
          let! _ = await (client.PSETEXAsync(key,expiration,value ))
          do! Async.Sleep 200
          let! _ = await (client.DELAsync key)
          client.quit()
          Expect.equal true false "should fail"
        with _ -> 
          client.quit()
          Expect.equal true true "ok"
      }

    testCaseAsync "SET with PX" <| 
      async {
        let client = redis.createClient() 
        try 
          let key = "setkey"
          let value = "Hello"
          let expiration = 100
          let! _ = await (client.SETAsync(key,value,PX,expiration ))
          do! Async.Sleep 200
          let! _ = await (client.DELAsync key)
          client.quit()
          Expect.equal true false "should fail"
        with _ -> 
          client.quit()
          Expect.equal true true "ok"
      }

    testCaseAsync "SET with NX" <| 
      async {
        let client = redis.createClient() 
        let key = "setNX"
        let value = "Hello"
        let expiration = 100
        let! result = await (client.SETAsync(key,value,NX))
        let! _ = await (client.DELAsync key)
        client.quit()
        Expect.equal "OK" result "ok"
      }      

    testCaseAsync "SET with NX 2" <| 
      async {
        let client = redis.createClient() 
        let key = "setNX2"
        let value = "Hello"
        let expiration = 100
        let! _ = await (client.SETAsync(key,value))
        let! result = await (client.SETAsync(key,value,NX,PX,expiration))
        let result =  result = null 
        let! _ = await (client.DELAsync key)
        client.quit()
        Expect.isTrue result "ok"
      }      

    testCaseAsync "SETRANGE" <| 
      async {
        let client = redis.createClient() 
        let key = "setRange"
        let value = "Hello World"
        let! _ = await (client.SETAsync(key,value))
        let! result = await (client.SETRANGEAsync(key,6,"Redis"))
        let result =  11 - result = 0
        let! _ = await (client.DELAsync key)
        client.quit()
        Expect.isTrue result "ok"
      }

    testCaseAsync "STRLEN" <| 
      async {
        let client = redis.createClient() 
        let key = "STRLEN"
        let value = "Hello World"
        let! _ = await (client.SETAsync(key,value))
        let! result = await (client.STRLENAsync(key))
        let result =  11 - result = 0
        let! _ = await (client.DELAsync key)
        client.quit()
        Expect.isTrue result "ok"
      }

    testCaseAsync "ECHO" <| 
      async {
        let client = redis.createClient() 
        let key = "ECHO"
        let! result = await (client.ECHOAsync(key))
        let result = result = key
        client.quit()
        Expect.isTrue result "ok"
      }      

    testCaseAsync "PING/PONG" <| 
      async {
        let client = redis.createClient() 
        let! result = await (client.PINGAsync())
        let result = result = "PONG"
        client.quit()
        Expect.isTrue result "ok"
      }            

    testCaseAsync "PING" <| 
      async {
        let client = redis.createClient() 
        let value = "test"
        let! result = await (client.PINGAsync value)
        let result = result = value
        client.quit()
        Expect.isTrue result "ok"
      }            

    testCaseAsync "EXISTS 1" <| 
      async {
        let client = redis.createClient() 
        let key = "EXISTS 1"
        let value = "Hello World"
        let! _ = await (client.SETAsync(key,value))
        let! result = await (client.EXISTSAsync(key))
        let result =  result = 1
        let! _ = await (client.DELAsync key)
        client.quit()
        Expect.isTrue result "ok"
      }

    testCaseAsync "EXISTS 2" <| 
      async {
        let client = redis.createClient() 
        let key = "EXISTS 2"
        let value = "Hello World"
        let! _ = await (client.SETAsync(key,value))
        let! result = await (client.EXISTSAsync("??"))
        let result =  result = 0
        let! _ = await (client.DELAsync key)
        client.quit()
        Expect.isTrue result "ok"
      }

    testCaseAsync "EXPIRE/TTL" <| 
      async {
        let client = redis.createClient() 
        let key = "EXPIRE"
        let value = "Hello World"
        let! _ = await (client.SETAsync(key,value))
        let! _ = await (client.EXPIREAsync(key,10))
        let! result = await (client.TTLAsync(key))
        let result =  result = 10
        let! _ = await (client.DELAsync key)
        client.quit()
        Expect.isTrue result "ok"
      }

    testCaseAsync "EXPIREAT" <| 
      async {
        let client = redis.createClient() 
        let key = "EXPIREAT"
        let value = "Hello World"
        let! _ = await (client.SETAsync(key,value))
        let! _ = await (client.EXPIREATAsync(key,1293840000))
        let! result = await (client.EXISTSAsync(key))
        let result =  result = 0
        let! _ = await (client.DELAsync key)
        client.quit()
        Expect.isTrue result "ok"
      }

    testCaseAsync "KEYS" <| 
      async {
        let client = redis.createClient() 
        let key = "foo"
        let value = "bar"        
        let keys = 
          [0..3] 
          |> Seq.map( fun i -> sprintf "%s%i" key i) 
          |> Seq.toArray
        
        let data = 
          keys 
          |> Seq.mapi( fun i k -> [k; sprintf "%s%i" value i])
          |> Seq.concat
          |> Seq.toArray

        let! _  = await (client.MSETAsync data)
        let! result = await (client.KEYSAsync("foo?"))
        let result =  result.Length = 4
        let! _ = await (client.DELAsync keys)
        client.quit()
        Expect.isTrue result "ok"
      }

    testCaseAsync "PERSIST" <| 
      async {
        let client = redis.createClient() 
        let key = "PERSIST"
        let value = "Hello World"
        let! _ = await (client.SETAsync(key,value))
        let! _ = await (client.EXPIREAsync(key,10))
        let! _ = await (client.PERSISTAsync(key))
        let! result = await (client.TTLAsync(key))
        let result =  result = -1
        let! _ = await (client.DELAsync key)
        client.quit()
        Expect.isTrue result "ok"
      }

    testCaseAsync "RANDOMKEY" <| 
      async {
        let client = redis.createClient() 
        let key = "RANDOMKEY"
        let value = "Hello World"
        let! _ = await (client.SETAsync(key,value))
        let! result = await (client.RANDOMKEYAsync())
        let result =  result <> null
        let! _ = await (client.DELAsync key)
        client.quit()
        Expect.isTrue result "ok"
      }

    testCaseAsync "RENAME" <| 
      async {
        let client = redis.createClient() 
        let key = "RENAME"
        let newkey = "RENAMED"
        let value = "Hello World"
        let! _ = await (client.SETAsync(key,value))
        let! _ = await (client.RENAMEAsync(key,newkey))
        let! result = await (client.GETAsync(newkey))
        let result =  result = "Hello World"
        let! _ = await (client.DELAsync key)
        client.quit()
        Expect.isTrue result "ok"
      }
  ]

Mocha.runTests tests |> ignore
