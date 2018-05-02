using ApiAiSdkNetCore.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAiSdkNetCore.Model
{
    [JsonObject]
    public class OriginalRequest
    {

        [JsonProperty("source")]
        public Provider Source { get; set; }

        // This property will get populate from the equivilent value
        [JsonProperty("data")]
        public JObject Data { get; set; }

        public Event Event {get;set;}
    }

    public class Event
    {

    }
}
