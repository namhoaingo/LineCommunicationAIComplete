using ApiAiSdkNetCore.Model;
using Bot_LineMessagingApi.SDK.Enums;
using Bot_LineMessagingApi.SDK.Models;
using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_LineMessagingApi.SDK.Managers
{
    public class IntentManager : IIntentManager
    {
        private readonly BotCredential _botCredential;
        private ITokenManager _tokenManager;
        private LineMessagingClient _lineMessagingClient;
        public IntentManager(IOptions<BotCredential> botCredential, ITokenManager tokenManager)
        {
            this._tokenManager = tokenManager;
            this._botCredential = botCredential.Value;
        }

        public async Task ProcessIntent(AIResponse aIResponse)
        {
            AccessToken accessToken = await _tokenManager.GetAccessTokenFromCache().ConfigureAwait(false);
            _lineMessagingClient = new LineMessagingClient(accessToken.access_token);
            var app = new LineBotApp(_lineMessagingClient);

            // try case IntentName to IntentType Enum
            IntentType intentType = IntentType.Unknown;

            if (Enum.TryParse<IntentType>(aIResponse.Result.Metadata.IntentName, out intentType))
            {
                aIResponse.Result.Metadata.IntentType = intentType;
            }

            Event tempEvent = new Event();
            switch (aIResponse.Result.Metadata.IntentType)
            {
                case IntentType.ButtonIntent:
                    // Down Cast

                    tempEvent = aIResponse.OriginalRequest.Event;
                    MessageEvent messageEventTemp = (MessageEvent)tempEvent;
                    TextEventMessage eventMessageTemp = (TextEventMessage)messageEventTemp.Message;
                    eventMessageTemp.Text = "buttons";

                    break;
                default:
                    break;
            }

            await app.RunAsync(new List<WebhookEvent>(){ (WebhookEvent)tempEvent });
        }
    }
}
