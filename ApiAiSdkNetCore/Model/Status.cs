using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAiSdkNetCore.Model
{
    [JsonObject]
    public class Status
    {
        [JsonProperty("code")]
        public int? Code { get; set; }

        [JsonProperty("errorType")]
        public string ErrorType { get; set; }

        [JsonProperty("errorDetails")]
        public string ErrorDetails { get; set; }

        [JsonProperty("errorID")]
        public string ErrorID { get; set; }

        public Status()
        {
        }
    }
}

