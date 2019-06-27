module rec Fable.Redis

open System
open Fable.Core
open Fable
open Fable.Core.JS

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
  abstract APPENDAsync : key:string * value:string -> Promise<int>
  abstract BITCOUNTAsync : key:string -> Promise<int>
  abstract BITCOUNTAsync : key:string * start:int * ``end``:int -> Promise<int>
  // BITFIELD
  // BITOP
  abstract BITPOSAsync : key:string * start:int * ``end``:int -> Promise<int>
  abstract DECRAsync : key:string -> Promise<int>
  abstract DECRBYAsync : key:string * decrement:int -> Promise<int>
  abstract GETAsync : key:string -> Promise<string>
  abstract GETBITAsync : key:string * offset:int -> Promise<int>
  abstract GETRANGEAsync : key:string * start:int * ``end``:int -> Promise<string>
  abstract GETSETAsync : key:string * value:string -> Promise<string>
  abstract INCRAsync : key:string -> Promise<int>
  abstract INCRBYAsync : key:string * increment:int -> Promise<int>
  abstract INCRBYFLOATAsync : key:string * increment:float -> Promise<float>
  abstract MGETAsync : keys:string []-> Promise<string []>
  abstract MSETAsync : keys:string []-> Promise<string>
  abstract MSETNXAsync : keys:string []-> Promise<string>
  abstract PSETEXAsync : key:string * milliseconds:int * value:string -> Promise<float>
  abstract SETAsync : key:string * value:string -> Promise<string>
  abstract SETAsync : key:string * value:string * option:SetOption * option:SetOption * param:int -> Promise<string>
  abstract SETAsync : key:string * value:string * option:SetOption * param:int -> Promise<string>
  abstract SETAsync : key:string * value:string * option:SetOption -> Promise<string>
  abstract SETAsync : string [] -> Promise<string>
  abstract SETBITAsync : key:string * offset:int * value:int -> Promise<int>
  // SETEX : use SET + EX
  // SETNX : use SET + NX
  abstract SETRANGEAsync : key:string * start:int * value:string -> Promise<int>
  abstract STRLENAsync : key:string -> Promise<int>
  
  // ===================> KEYS
  abstract DELAsync : key:string -> Promise<int>
  abstract DELAsync : keys:string []-> Promise<int>
  abstract DUMPAsync : key:string-> Promise<string>
  abstract EXISTSAsync : key:string -> Promise<int>
  abstract EXPIREAsync : key:string * timeout:int -> Promise<int>
  abstract EXPIREATAsync : key:string * timestamp:int -> Promise<int>
  abstract KEYSAsync : key:string -> Promise<string []>
  abstract MOVEAsync : key:string * db:int -> Promise<int>
  abstract PERSISTAsync : key:string -> Promise<int>
  abstract PEXPIREAsync : key:string * timeout:int -> Promise<int>
  abstract PEXPIREATAsync : key:string * timestamp:int -> Promise<int>
  abstract PTTLAsync : key:string -> Promise<int>
  abstract RANDOMKEYAsync : unit -> Promise<string>
  abstract RENAMEAsync : key:string * newkey:string -> Promise<int>
  abstract RENAMENXAsync : key:string * newkey:string -> Promise<int>
  // RESTORE
  // SORT
  // TOUCH
  abstract TTLAsync : key:string -> Promise<int>
  // TYPE
  // UNLINK
  // WAIT
  // SCAN

  // ===================> LISTS
  // BLPOP
  // BRPOP
  // BRPOPLPUSH
  abstract LINDEXAsync : listName:string * index:int -> Promise<string>
  //LINSERT
  abstract LLENAsync : listName:string -> Promise<int>
  abstract LPOPAsync : listName:string -> Promise<string>
  abstract LPUSHAsync : listName:string * value:string -> Promise<int>
  abstract LPUSHXAsync : listName:string * value:string -> Promise<int>
  abstract LRANGEAsync : listName:string * start:int * ``end``:int -> Promise<string []>
  abstract LREMAsync : listName:string * count:int * value:string -> Promise<int>
  abstract LSETAsync : listName:string * index:int * value:string -> Promise<string>
  abstract LTRIMAsync : listName:string * start:int * ``end``:int -> Promise<string>
  abstract RPOPAsync : listName:string -> Promise<string>
  abstract RPOPLPUSHAsync : listName:string * listName:string -> Promise<string>
  abstract RPUSHAsync : listName:string * value:string -> Promise<int>
  abstract RPUSHXAsync : listName:string * value:string -> Promise<int>

  // ===================> HASHES
  abstract HSETAsync : hashName:string * key:string * value:string -> Promise<int>
  abstract HSETAsync : data:string [] -> Promise<int>
  abstract HDELAsync : hashName:string * fieldName:string -> Promise<int>
  abstract HEXISTSAsync : hashName:string * fieldName:string -> Promise<int>
  abstract HGETASync : hashName:string * fieldName:string -> Promise<string>
  abstract HGETALLASync : hashName:string -> Promise<string []>
  abstract HINCRBYASync : hashName:string * fieldName:string * increment:int -> Promise<int>
  abstract HINCRBYFLOATASync : hashName:string * fieldName:string * increment:float -> Promise<float>
  abstract HKEYSASync : hashName:string -> Promise<string []>
  abstract HLENASync : hashName:string -> Promise<int>
  abstract HMGETASync : data:string [] -> Promise<string []>
  abstract HMSETASync : data:string [] -> Promise<string>
  // HSCAN
  abstract HSETNXAsync : hashName:string * key:string * value:string -> Promise<int>
  abstract HSETNXAsync : data:string [] -> Promise<int>
  abstract HSTRLENASync : hashName:string * fieldName:string -> Promise<int>
  abstract HVALSASync : hashName:string -> Promise<string []>

  // ===================> SETS
  abstract SADDAsync : setName:string * value:string -> Promise<int>
  abstract SCARDAsync : setName:string -> Promise<int>
  abstract SDIFFAsync : keys:string [] -> Promise<string []>
  abstract SDIFFSTOREAsync : keys:string [] -> Promise<int>
  abstract SINTERAsync : keys:string [] -> Promise<string []>
  abstract SINTERSTOREAsync : keys:string [] -> Promise<string []>
  abstract SISMEMBERAsync : setName:string * value:string -> Promise<int>
  abstract SMEMBERSAsync : setName:string -> Promise<string []>
  abstract SMOVEAsync : fromSet:string * toSet:string * value:string -> Promise<int>
  abstract SPOPAsync : setName:string -> Promise<string>
  abstract SRANDMEMBERAsync : setName:string -> Promise<string>
  abstract SRANDMEMBERAsync : setName:string * index:int -> Promise<string []>
  abstract SREMAsync : setName:string * value:string -> Promise<int>
  // SSCAN
  abstract SUNIONAsync : keys:string [] -> Promise<string []>
  abstract SUNIONSTOREAsync : keys:string [] -> Promise<int>
  
  // ===================> SORTED SETS
  // BZPOPMAX
  // BZPOPMIN
  abstract ZADDAsync : setName:string * fieldName:string * value:string -> Promise<int>
  abstract ZADDAsync : data:string [] -> Promise<int>
  abstract ZCARDAsync : setName:string -> Promise<int>
  abstract ZINCRBYAsync : setName:string * increment:int * value:string -> Promise<int>
  abstract ZCOUNTAsync : setName:string * rangeFrom:int * rangeTo:int-> Promise<int>
  abstract ZCOUNTAsync : data:string [] -> Promise<int>
  // ZINTERSTORE
  abstract ZLEXCOUNTAsync : data:string [] -> Promise<int>
  abstract ZPOPMAX : setName:string -> Promise<string []>
  abstract ZPOPMIN : setName:string -> Promise<string []>
  abstract ZRANGEAsync : key:string * startIndex:int * endIndex:int -> Promise<string []>
  abstract ZRANGEAsync : data:string [] -> Promise<string []>
  // ZRANGEBYLEX
  abstract ZRANGEBYSCOREAsync : data:string [] -> Promise<string []>
  abstract ZRANKAsync : setName:string * fieldName:string -> Promise<int>  
  abstract ZREMAsync : setName:string * fieldName:string -> Promise<int>  
  // ZREMRANGEBYLEX
  // ZREMRANGEBYRANK
  // ZREMRANGEBYSCORE
  // ZREVRANGE
  // ZREVRANGEBYLEX
  // ZREVRANGEBYSCORE
  abstract ZREVRANKAsync : setName:string * fieldName:string -> Promise<int>  
  // ZSCAN
  abstract ZSCOREAsync : setName:string * fieldName:string -> Promise<string>  
  // ZUNIONSTORE


  // ===================> CONNECTION
  // AUTH
  abstract ECHOAsync : key:string -> Promise<string>
  abstract PINGAsync : key:string -> Promise<string>
  abstract PINGAsync : unit -> Promise<string>
  // QUIT
  // SELECT
  abstract SWAPDBAsync : db1:int * db2:int -> Promise<string>

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
