module rec Fable.Redis

open System
open Fable.Core
open Fable

let [<Import("*","redis")>] redis: Static = jsNative

type Client = 
  abstract set : k:string -> value:string -> unit
  abstract auth: unit -> unit
  abstract quit: unit -> unit
  abstract quitAsync: unit -> JS.Promise<unit>
  abstract setAsync : k:string * value:string -> JS.Promise<string>
  //abstract HSETAsync : HashName * obj -> JS.Promise<int>
  //abstract HSETAsync : (HashName * Key * Value) -> JS.Promise<int>
  abstract HSETAsync : string [] -> JS.Promise<int>
  abstract authAsync: unit -> JS.Promise<string>
  abstract selectAsync: dbIndex:int -> JS.Promise<string>
  abstract getAsync: k:string -> JS.Promise<string>
  abstract HGETALLAsync : hashName:string -> JS.Promise<obj>
  abstract HDELAsync : hashName:string*fieldName:string -> JS.Promise<int>
  abstract HDELAsync : string [] -> JS.Promise<int>
  
type ClientOptions = 
  abstract host: string with get, set 
  abstract port: int with get, set 
  abstract path: string option with get, set 
  abstract url: string option with get, set 
  abstract socket_keepalive: bool with get, set
  abstract socket_initialdelay: int with get, set
  abstract password: string option with get, set
  abstract tls: Node.Tls.TlsOptions option with get,set
  
type Static = 
    abstract createClient: ?options:ClientOptions -> Client
    abstract createClient: url:string * ?options:ClientOptions -> Client
