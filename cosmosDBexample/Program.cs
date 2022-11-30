using Microsoft.Azure.Cosmos;


Console.WriteLine("Hello, Cosmos!");

var uri = "https://zoomcat.documents.azure.com:443/";
var key = "[keygoeshere]";


CosmosClient client = new CosmosClient(uri, key);

var dbResponse = await client.CreateDatabaseIfNotExistsAsync("ZoomCatDB");
var db = dbResponse.Database;

var containerResponse = await db.CreateContainerIfNotExistsAsync("ZoomCatContainer", "/address/county");
var container = containerResponse.Container;


var emp1 = new { id = "1", name = "John", age = 30, address = new { state = "WA", county = "King", city = "Seattle", street = "123 main" } };
var emp2 = new { id = "2", name = "Bob", age = 35, address = new { state = "WA", county = "King", city = "Redmond", street = "456 1st ave NE" } };
var emp3 = new { id = "3", name = "Mike", age = 40, address = new { state = "WA", county = "King", city = "Bellevue", street = "789 2nd ave NE" } };

var emp1Response = await container.CreateItemAsync(emp1);
Console.WriteLine($"{emp1.name} created with resource unit {emp1Response.RequestCharge}");


var emp2Response = await container.CreateItemAsync(emp2);


var emp3Response = await container.CreateItemAsync(emp3);


