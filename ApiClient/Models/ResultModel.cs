using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiClient.Models
{
    public record ResultModel (string isin, string status)
    {
    }
}