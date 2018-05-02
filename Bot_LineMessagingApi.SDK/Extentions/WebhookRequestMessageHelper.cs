using Line.Messaging;
using Line.Messaging.Webhooks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bot_LineMessagingApi.SDK.Extentions
{
    public static class WebhookRequestMessageHelper
    {
        // This is only for Line
        public static WebhookEvent GetWebhookEventFromStringAsync(JObject data)
        {
            if (data == null) { throw new ArgumentNullException(nameof(data)); }

            WebhookEvent result = WebhookEventParser.ParseLineFromDialogFlow(data.ToString(Newtonsoft.Json.Formatting.None));

            return result;
        }


        /// <summary>
        /// Verify if the request is valid, then returns LINE Webhook events from the request
        /// </summary>
        /// <param name="request">HttpRequestMessage</param>
        /// <param name="channelSecret">ChannelSecret</param>
        /// <returns>List of WebhookEvent</returns>
        public static async Task<IEnumerable<WebhookEvent>> GetWebhookEventsAsync(this HttpRequest request, string channelSecret)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (channelSecret == null) { throw new ArgumentNullException(nameof(channelSecret)); }

            var streamReader = new StreamReader(request.Body);
            var content = await Task.Run(() => streamReader.ReadToEnd());

            var xLineSignature = request.Headers["X-Line-Signature"];
            if (string.IsNullOrEmpty(xLineSignature) || !VerifySignature(channelSecret, xLineSignature, content))
            {
                throw new InvalidSignatureException("Signature validation faild.");
            }
            return WebhookEventParser.Parse(content);
        }

        /// <summary>
        /// The signature in the X-Line-Signature request header must be verified to confirm that the request was sent from the LINE Platform.
        /// Authentication is performed as follows.
        /// 1. With the channel secret as the secret key, your application retrieves the digest value in the request body created using the HMAC-SHA256 algorithm.
        /// 2. The server confirms that the signature in the request header matches the digest value which is Base64 encoded
        /// https://developers.line.me/en/docs/messaging-api/reference/#signature-validation
        /// </summary>
        /// <param name="channelSecret">ChannelSecret</param>
        /// <param name="xLineSignature">X-Line-Signature header</param>
        /// <param name="requestBody">RequestBody</param>
        /// <returns></returns>
        internal static bool VerifySignature(string channelSecret, string xLineSignature, string requestBody)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(channelSecret);
                var body = Encoding.UTF8.GetBytes(requestBody);

                using (HMACSHA256 hmac = new HMACSHA256(key))
                {
                    var hash = hmac.ComputeHash(body, 0, body.Length);
                    var xLineBytes = Convert.FromBase64String(xLineSignature);
                    return SlowEquals(xLineBytes, hash);
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Compares two-byte arrays in length-constant time. 
        /// This comparison method is used so that password hashes cannot be extracted from on-line systems using a timing attack and then attacked off-line.
        /// <remarks> http://bryanavery.co.uk/cryptography-net-avoiding-timing-attack/#comment-85　</remarks>
        /// </summary>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }
    }
}


