var redis = require("redis"),
    client = redis.createClient();
client.set("string key", "string val");
let get = client.get("string key", (err, data) => console.log(data) );
console.log(get)
client.quit();
