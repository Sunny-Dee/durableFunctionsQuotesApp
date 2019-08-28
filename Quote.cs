using System;
using System.Collections.Generic;
using System.Text;

namespace QuotesApp
{
    public class Quote
    {
        public IList<string> tags { get; set; }
        public string quote { get; set; }
        public string author { get; set; }
    }
}
