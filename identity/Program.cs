using System.Net.Http.Headers;
using Microsoft.Identity.Client;


Console.WriteLine("Hello, Microsoft Identity!");

var clientId = "e95a3432-e21f-47ac-a18a-3002bb8edeba";
var tenantId = "535e8246-b18f-4802-9b11-babf6a477647";

var client = PublicClientApplicationBuilder.Create(clientId)
    .WithTenantId(tenantId)
    .WithRedirectUri("http://localhost")
    .Build();

var scopes = new[] { "User.Read" };

var result = await client.AcquireTokenInteractive(scopes)
    .ExecuteAsync();

Console.WriteLine(result.AccessToken);
Console.WriteLine();
Console.WriteLine(result.IdToken);

var httpClient = new HttpClient() { BaseAddress = new Uri("https://graph.microsoft.com/v1.0/") };

httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

var response = await httpClient.GetAsync("me");
var content = await response.Content.ReadAsStringAsync();
Console.WriteLine(content);