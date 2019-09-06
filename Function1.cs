using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace QuotesApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "hello")] HttpRequest req,
            [OrchestrationClient] DurableOrchestrationClient starter,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var orchId = await starter.StartNewAsync("O_LoggerOrchestrator", name);

            return name != null
                ? (Microsoft.AspNetCore.Mvc.ActionResult)new OkObjectResult($"hello, {name}")
                : new BadRequestObjectResult("please pass a name on the query string or in the request body");
        }

        [FunctionName("O_LoggerOrchestrator")]
        public static async Task<IActionResult> HelloOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context, 
            ILogger log)
        {
            var name = context.GetInput<string>();
            log.LogWarning("in orchestrator logging this message");
            await context.CallActivityAsync("LogTest", $"{name}: such a cool message");

            return new OkResult();
        }


        [FunctionName("LogTest")]
        public static async Task TestLogging(
            [ActivityTrigger] DurableActivityContext inputs)
        {
            string message = inputs.GetInput<string>();

            ILog log1 = new LogImplementation("Log 1");
            ILog log2 = new LogImplementation("Log 2");

            var collector = new CollectorExtension
            {
                log1,
                log2
            };

            await collector.LogInfoForAll(message);
        }
    }
}
