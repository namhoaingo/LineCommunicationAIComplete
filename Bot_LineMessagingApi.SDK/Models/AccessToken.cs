using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_LineMessagingApi.SDK.Models
{
    public class AccessToken
    {
        public string access_token { get; set; }
        public long expires_in { get; set; }
        public string token_type { get; set; }
    }
}
