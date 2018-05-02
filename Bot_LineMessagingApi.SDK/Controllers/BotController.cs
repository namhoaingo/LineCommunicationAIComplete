using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiAiSdkNetCore.Model;
using Bot_LineMessagingApi.SDK.Extentions;
using Bot_LineMessagingApi.SDK.Managers;
using Bot_LineMessagingApi.SDK.Models;
using Line.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Bot_LineMessagingApi.SDK.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BotController : Controller
    {
        private readonly BotCredential _botCredential;
        private readonly ITokenManager _tokenManager;
        private readonly IIntentManager _intentManager;
        private LineMessagingClient _messagingClient;

        public BotController(IOptions<BotCredential> botCredential, ITokenManager tokenManager, IIntentManager intentManager)
        {
            this._tokenManager = tokenManager;
            this._intentManager = intentManager;
            this._botCredential = botCredential.Value;            
        }

        [HttpPost]
        [ActionName("callback")]
        public async Task<IActionResult> Post()
        {
            AccessToken accessToken = await _tokenManager.GetAccessTokenFromCache().ConfigureAwait(false);
            _messagingClient = new LineMessagingClient(accessToken.access_token);

            var events = await Request.GetWebhookEventsAsync(_botCredential.AppSecret);
            var app = new LineBotApp(_messagingClient);
            await app.RunAsync(events);

            return Ok();
        }

        [HttpPost]
        [ActionName("dialogFlow")]
        public async Task<IActionResult> DialogFlowPost()
        {
            AIResponse response = null;
            using (var streamReader = new StreamReader(Request.Body))
            {
                response = JsonConvert.DeserializeObject<AIResponse>(streamReader.ReadToEnd());
                
            }

            switch(response.OriginalRequest.Source)
            {
                case ApiAiSdkNetCore.Enums.Provider.Line:
                    // populate the data object with the correct item
                    response.OriginalRequest.Event = WebhookRequestMessageHelper.GetWebhookEventFromStringAsync(response.OriginalRequest.Data);
                    break;
                default:
                    break;                    
            }

            await _intentManager.ProcessIntent(response);


            return Ok();
        }

    }
}
