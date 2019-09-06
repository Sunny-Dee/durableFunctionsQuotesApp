using System;
using System.Collections.Generic;
using System.Text;

namespace QuotesApp
{
    public static class Constants
    {
        public const string OrchestratorFunctionName = "O_ProcessQuoteRequest";
        public const string GetQuoteActivityName = "A_GetQuote";
        public const string FormatQuoteActivity = "A_FormatQuote";
        public const string WarningLoggingOrchestrator = "O_LogWarningMessage";
        public const string QuotesUrl = "https://opinionated-quotes-api.gigalixirapp.com/v1/quotes";
    }
}
