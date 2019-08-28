using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;

namespace QuotesApp
{
    public static class StartApp
    {
        [FunctionName("StartApp")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [OrchestrationClient] DurableOrchestrationClient starter,
            ILogger log)
        {
            // parse query parameter
            var tagString = req.GetQueryParameterDictionary().FirstOrDefault(q => q.Key == "tags").Value;

            IEnumerable<string> tags = new List<string>();
            if (!string.IsNullOrEmpty(tagString))
            {
                tags =  tagString.Split(",").ToList();
            }

            if (!tags.Any())
            {
                log.LogInformation("No tags specified. Returning a random quote");
            }
            else
            {
                log.LogInformation($"Starting to process with tags {string.Join(",", tags)}");
            }

            var orchestrationId = await starter.StartNewAsync(Constants.OrchestratorFunctionName, tags);

            var reqMessage = new HttpRequestMessage
            {
                Content = new StringContent(req.Path.ToString()),
            };
            return starter.CreateCheckStatusResponse(reqMessage, orchestrationId);
        }
    }
}
