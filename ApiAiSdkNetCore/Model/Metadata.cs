using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAiSdkNetCore.Model
{
    [JsonObject]
    public class Metadata
    {
        [JsonProperty("intentName")]
        public String IntentName { get; set; }

        [JsonProperty("intentId")]
        public string IntentId { get; set; }

        public Enum IntentType { get; set; }

        public Metadata()
        {
        }
    }
}

