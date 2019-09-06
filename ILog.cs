using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QuotesApp
{
    interface ILog
    {
        string Name { get; set; }
        Task LogInfo(string message);
    }
}
