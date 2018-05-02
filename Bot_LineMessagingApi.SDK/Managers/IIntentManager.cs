using ApiAiSdkNetCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_LineMessagingApi.SDK.Managers
{
    public interface IIntentManager
    {
        Task ProcessIntent(AIResponse aIResponse);
    }
}
