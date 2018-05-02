using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAiSdkNetCore.Model
{
    [JsonObject]
    public class AIResponse
    {
        [JsonProperty("originalRequest")]
        public OriginalRequest OriginalRequest { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("sessionId")]
        public string SessionId { get; set; }

        public bool IsError
        {
            get
            {
                if (Status != null && Status.Code.HasValue && Status.Code >= 400)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
