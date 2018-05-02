using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAiSdkNetCore.Model
{
    [JsonObject]
    public class EntityEntry
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("synonyms")]
        public List<String> Synonyms { get; set; }

        public EntityEntry()
        {
        }

        public EntityEntry(string value, List<string> synonyms)
        {
            this.Value = value;
            this.Synonyms = synonyms;
        }

        public EntityEntry(string value, string[] synonyms) : this(value, new List<string>(synonyms))
        {

        }

    }
}

