using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Line.Messaging
{
    //TODO:
    //After moving the code to a framework level
    //Change this to internal
    public class CamelCaseJsonSerializerSettings : JsonSerializerSettings
    {
        public CamelCaseJsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver();
            Converters.Add(new StringEnumConverter { CamelCaseText = true });
        }
    }
}
