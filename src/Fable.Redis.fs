module rec Fable.Redis

open System
open Fable.Core
open Fable

let [<Import("*","redis")>] redis: Static = jsNative
let [<Import("*","bluebird")>] bluebird: BlueBird = jsNative

type BlueBird = 
  abstract promisifyAll : redis:Redis.Static -> unit

[<StringEnum>]
type SetOption = 
  | [<CompiledName("EX")>] EX 
  | [<CompiledName("PX")>] PX 
  | [<CompiledName("NX")>] NX 
  | [<CompiledName("XX")>] XX

type Client = 

  // ===================> lib
  abstract quit: unit -> unit
  
  // ===================> STRING
  abstract APPENDAsync : key:string * value:string -> JS.Promise<int>
  abstract BITCOUNTAsync : key:string -> JS.Promise<int>
  abstract BITCOUNTAsync : key:string * start:int * ``end``:int -> JS.Promise<int>
  // BITFIELD
  // BITOP
  abstract BITPOSAsync : key:string * start:int * ``end``:int -> JS.Promise<int>
  abstract DECRAsync : key:string -> JS.Promise<int>
  abstract DECRBYAsync : key:string * decrement:int -> JS.Promise<int>
  abstract GETAsync : key:string -> JS.Promise<string>
  abstract GETBITAsync : key:string * offset:int -> JS.Promise<int>
  abstract GETRANGEAsync : key:string * start:int * ``end``:int -> JS.Promise<string>
  abstract GETSETAsync : key:string * value:string -> JS.Promise<string>
  abstract INCRAsync : key:string -> JS.Promise<int>
  abstract INCRBYAsync : key:string * increment:int -> JS.Promise<int>
  abstract INCRBYFLOATAsync : key:string * increment:float -> JS.Promise<float>
  abstract MGETAsync : keys:string []-> JS.Promise<string []>
  abstract MSETAsync : keys:string []-> JS.Promise<string>
  abstract MSETNXAsync : keys:string []-> JS.Promise<string>
  abstract PSETEXAsync : key:string * milliseconds:int * value:string -> JS.Promise<float>
  abstract SETAsync : key:string * value:string -> JS.Promise<string>
  abstract SETAsync : key:string * value:string * option:SetOption * option:SetOption * param:int -> JS.Promise<string>
  abstract SETAsync : key:string * value:string * option:SetOption * param:int -> JS.Promise<string>
  abstract SETAsync : key:string * value:string * option:SetOption -> JS.Promise<string>
  abstract SETAsync : string [] -> JS.Promise<string>
  abstract SETBITAsync : key:string * offset:int * value:int -> JS.Promise<int>
  // SETEX : use SET + EX
  // SETNX : use SET + NX
  abstract SETRANGEAsync : key:string * start:int * value:string -> JS.Promise<int>
  abstract STRLENAsync : key:string -> JS.Promise<int>
  
  // ===================> KEYS
  abstract DELAsync : key:string -> JS.Promise<int>
  abstract DELAsync : keys:string []-> JS.Promise<int>
  abstract DUMPAsync : key:string-> JS.Promise<string>
  abstract EXISTSAsync : key:string -> JS.Promise<int>
  abstract EXPIREAsync : key:string * timeout:int -> JS.Promise<int>
  abstract EXPIREATAsync : key:string * timestamp:int -> JS.Promise<int>
  abstract KEYSAsync : key:string -> JS.Promise<string []>
  abstract MOVEAsync : key:string * db:int -> JS.Promise<int>
  abstract PERSISTAsync : key:string -> JS.Promise<int>
  abstract PEXPIREAsync : key:string * timeout:int -> JS.Promise<int>
  abstract PEXPIREATAsync : key:string * timestamp:int -> JS.Promise<int>
  abstract PTTLAsync : key:string -> JS.Promise<int>
  abstract RANDOMKEYAsync : unit -> JS.Promise<string>
  abstract RENAMEAsync : key:string * newkey:string -> JS.Promise<int>
  abstract RENAMENXAsync : key:string * newkey:string -> JS.Promise<int>
  // RESTORE
  // SORT
  // TOUCH
  abstract TTLAsync : key:string -> JS.Promise<int>
  // TYPE
  // UNLINK
  // WAIT
  // SCAN

  // ===================> LISTS
  // ===================> HASHES
  // ===================> SETS
  // ===================> SORTED SETS


  // ===================> CONNECTION
  // AUTH
  abstract ECHOAsync : key:string -> JS.Promise<string>
  abstract PINGAsync : key:string -> JS.Promise<string>
  abstract PINGAsync : unit -> JS.Promise<string>
  // QUIT
  // SELECT
  abstract SWAPDBAsync : db1:int * db2:int -> JS.Promise<string>

  // ===================> SCRIPTING
  // ===================> PUBUB
  // ===================> SERVER
  // ===================> HYPERLOG
  // ===================> GEO
  // ===================> STREAMS
  // ===================> TRANSACTIONS

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
