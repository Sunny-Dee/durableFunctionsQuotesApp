using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QuotesApp
{
    public static class QuoteRequestProcessor
    {
        [FunctionName(Constants.OrchestratorFunctionName)]
        public static async Task<object> ProcessQuoteRequest(
            [OrchestrationTrigger] DurableOrchestrationContext context, 
            ILogger log)
        {
            var tags = context.GetInput<List<string>>();

            var quote = await context.CallActivityAsync<string>(Constants.GetQuoteActivityName, tags);
            if (!context.IsReplaying)
            {
                log.LogInformation($"Got quote {quote}");
            }

            var formattedQuote = await context.CallActivityAsync<string>(Constants.FormatQuoteActivity, quote);
            if (!context.IsReplaying)
            {
                log.LogInformation($"Successfullly formatted quote {formattedQuote}");
            }

            return new
            {
                Quote = quote,
                Formatted = formattedQuote
            };
        }
    }
}
