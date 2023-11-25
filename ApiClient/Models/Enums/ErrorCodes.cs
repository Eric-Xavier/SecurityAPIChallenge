using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiClient.Models.Enums
{
    [JsonConverter(typeof(string))]
    public enum ErrorCodes
    {
        [JsonPropertyName("NoError")]
        NoError = 0,

        [JsonPropertyName("NotFoundInWebService")]
        NotFoundInWebService = -1,

        [JsonPropertyName("NotStoredInDatabase")]
        NotStoredInDatabase = -2,

        [JsonPropertyName("NotValidSecurityCode")]
        NotValidSecurityCode = -3,


        [JsonPropertyName("UnknownException")]
        UnknownException = -4
        
    }
}