using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiAiSdkNetCore.Model
{
    [JsonObject]
    public class Entity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("entries")]
        public List<EntityEntry> Entries { get; set; }

        public Entity()
        {
        }

        public Entity(string name)
        {
            this.Name = name;
        }

        public Entity(string name, List<EntityEntry> entries)
        {
            this.Name = name;
            this.Entries = entries;
        }

        public void AddEntry(EntityEntry entry)
        {
            if (Entries == null)
            {
                Entries = new List<EntityEntry>();
            }

            Entries.Add(entry);
        }

    }
}

