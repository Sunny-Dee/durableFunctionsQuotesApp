using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QuotesApp
{
    class LogImplementation : ILog
    {
        public LogImplementation(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public Task LogInfo(string message)
        {
            Console.WriteLine($"*** this is the logger  {Name}. Message: {message} ****");
            return Task.CompletedTask;
        }
    }
}
