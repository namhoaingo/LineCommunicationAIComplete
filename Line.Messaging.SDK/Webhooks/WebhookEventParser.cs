using ApiAiSdkNetCore.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Line.Messaging.Webhooks
{
    public static class WebhookEventParser
    {
        public static IEnumerable<WebhookEvent> Parse(string webhookContent)
        {
            dynamic dynamicObject = JsonConvert.DeserializeObject(webhookContent);
            if (dynamicObject == null) { yield break; }

            foreach (var ev in dynamicObject.events)
            {
                var webhookEvent = WebhookEvent.CreateFrom(ev);
                if (webhookEvent == null) { continue; }
                yield return webhookEvent;
            }
        }

        /// <summary>
        /// This function is still support Line but this Line message has been transfer through 
        /// Dialog FLOW
        /// </summary>
        /// <param name="webhookContent"></param>
        /// <returns></returns>
        public static WebhookEvent ParseLineFromDialogFlow(string webhookContent)
        {
            dynamic dynamicObject = JsonConvert.DeserializeObject(webhookContent);                      
            var webhookEvent = WebhookEvent.CreateFrom(dynamicObject);
            return webhookEvent;
        }
    }
}
