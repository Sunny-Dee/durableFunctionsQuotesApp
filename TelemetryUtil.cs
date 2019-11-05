using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuotesApp
{
    public static class TelemetryUtil
    {
        private static TelemetryClient telemetryClient;

        private static string _intrumentationKey;
        private static string instrumentationKey
        {
            get
            {
                if (_intrumentationKey == null)
                {
                    _intrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");
                }

                return _intrumentationKey;
            }
        }

        public static bool InstrumentationKeySet
        {
            get
            {
                return !string.IsNullOrEmpty(instrumentationKey); 
            }
        }

        [Obsolete]
        public static TelemetryClient TelemetryClient
        {
            get
            {
                if (telemetryClient == null)
                {
                    telemetryClient = new TelemetryClient
                    {
                        InstrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY")
                    };
                }
                return telemetryClient;
            }
        }


    }
}
