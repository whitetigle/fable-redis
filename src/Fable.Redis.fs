module rec Fable.Redis

open System
open Fable.Core
open Fable

let [<Import("*","redis")>] redis: Static = jsNative
let [<Import("*","bluebird")>] bluebird: BlueBird = jsNative

type BlueBird = 
  abstract promisifyAll : redis:Redis.Static -> unit

type Client = 

  // SYNC
  abstract quit: unit -> unit
  
  abstract SETAsync : key:string * value:string -> JS.Promise<int>
  abstract SETAsync : string [] -> JS.Promise<int>
  abstract GETAsync : key:string -> JS.Promise<string>

// TODO: check is this is the real object returned
type RetryStrategyParameter = 
  abstract attempt: int with get
  abstract total_retry_time: float with get
  abstract error: string with get
  abstract times_connected: int with get

type ClientOptions = 
  abstract host: string with get, set 
  abstract port: int with get, set 
  abstract path: string option with get, set 
  abstract url: string option with get, set 
  abstract string_numbers: bool option with get, set
  abstract return_buffers: bool with get, set
  abstract detect_buffers: bool with get, set
  abstract socket_keepalive: bool with get, set
  abstract socket_initialdelay: int with get, set
  abstract enable_offline_queue: int with get, set
  abstract retry_unfulfilled_commands: int with get, set  
  abstract password: string option with get, set
  abstract no_ready_check: bool with get, set
  abstract db: int option with get, set   
  
  // TODO, update Fable.Node package 
  // abstract family: XXX with get, set
  abstract disable_resubscribing: bool with get, set  

  abstract rename_commands: obj with get, set
  abstract tls: Node.Tls.TlsOptions option with get,set
  abstract prefix: string with get, set

  // TODO: check if return values change behaviors
  abstract retry_strategy: RetryStrategyParameter -> U2<int,obj> with get, set
  
type Static = 
    abstract createClient: ?options:ClientOptions -> Client
    abstract createClient: url:string * ?options:ClientOptions -> Client
