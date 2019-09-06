using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace QuotesApp
{
    class CollectorExtension : Collection<ILog>
    {
        public async Task LogInfoForAll(string msg)
        {
            var tasks = new List<Task>();
           foreach (var logger in Items)
            {
                //Console.WriteLine($"Console: logging {msg} for {logger.Name}");
                tasks.Add(logger.LogInfo(msg));
            }
            await Task.WhenAll(tasks);
        }
    }
}
