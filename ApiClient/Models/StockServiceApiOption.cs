using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public class StockServiceApiOption
    {
        public string BaseUrl { get; set; }
        public string GetUrl { get; set; }
        public int Timeout { get; set; }
    }
}