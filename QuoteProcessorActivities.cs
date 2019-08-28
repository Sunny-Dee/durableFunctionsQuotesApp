using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuotesApp
{
    public static class QuoteProcessorActivities
    {
        private static HttpClientHandler clientHandler = new HttpClientHandler();

        [FunctionName(Constants.GetQuoteActivityName)]
        public static async Task<string> GetQuote(
            [ActivityTrigger] List<string> tags,
            ILogger log)
        {
            var quote = string.Empty;
            using (var client = new HttpClient(clientHandler, false))
            {
                var requestUri = tags.Count == 0 ? Constants.QuotesUrl : $"{Constants.QuotesUrl}?tags={string.Join(",", tags)}";
                
                try
                {
                    var response = await client.GetAsync(requestUri);

                    log.LogInformation($"Got response from {requestUri}. Status code {response.StatusCode}");

                    quote = response == null ? "No quote for this query" : await response.Content.ReadAsStringAsync();
                }
                catch (Exception e)
                {
                    log.LogError(e, "Found exception when retrieving quote");
                }
                return quote;
            }
        }

        [FunctionName(Constants.FormatQuoteActivity)]
        public static string FormatQuote(
            [ActivityTrigger] string quote,
            ILogger log)
        {
            var q = JsonConvert.DeserializeAnonymousType(quote, new { quotes = new[] { new Quote() } });

            if (q.quotes.Length == 0)
            {
               return "Quote could not be formatted.";
            }

            var response = $"{q.quotes[0].quote} - {q.quotes[0].author}";
            log.LogInformation($"[FINAL QUOTE] {response}");
            
            return response;
        }
    }
}
