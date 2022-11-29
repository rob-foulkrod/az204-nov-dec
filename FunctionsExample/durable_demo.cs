using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace zoomcat
{
    public static class durable_demo
    {
        [FunctionName("durable_demo_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("durable_demo", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }




        [FunctionName("durable_demo")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>("durable_demo_Hello", "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>("durable_demo_Hello", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>("durable_demo_Hello", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("durable_demo_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {

            System.Threading.Thread.Sleep(5000);

            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }


    }
}