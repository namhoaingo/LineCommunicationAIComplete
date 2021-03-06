﻿using Line.Messaging;
using Line.Messaging.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_LineMessagingApi.SDK
{
    internal class LineBotApp: WebhookApplication
    {
        private LineMessagingClient messagingClient { get; }
        
        public LineBotApp(LineMessagingClient lineMessagingClient)
        {
            this.messagingClient = lineMessagingClient;            
        }

        #region Handlers

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            switch (ev.Message.Type)
            {
                case EventMessageType.Text:
                    await HandleTextAsync(ev.ReplyToken, ((TextEventMessage)ev.Message).Text, ev.Source.UserId);
                    break;
                case EventMessageType.Image:
                case EventMessageType.Audio:
                case EventMessageType.Video:
                case EventMessageType.File:
                    // Prepare blob directory name for binary object.
                    var blobDirectoryName = ev.Source.Type + "_" + ev.Source.Id;
                    await HandleMediaAsync(ev.ReplyToken, ev.Message.Id, blobDirectoryName, ev.Message.Id);
                    break;
                case EventMessageType.Location:
                    var location = ((LocationEventMessage)ev.Message);
                    await HandleLocationAsync(ev.ReplyToken, location);
                    break;
                case EventMessageType.Sticker:
                    await HandleStickerAsync(ev.ReplyToken);
                    break;
            }
        }

        protected override async Task OnPostbackAsync(PostbackEvent ev)
        {
            switch (ev.Postback.Data)
            {
                case "Date":
                    await messagingClient.ReplyMessageAsync(ev.ReplyToken,
                        "You chose the date: " + ev.Postback.Params.Date);
                    break;
                case "Time":
                    await messagingClient.ReplyMessageAsync(ev.ReplyToken,
                        "You chose the time: " + ev.Postback.Params.Time);
                    break;
                case "DateTime":
                    await messagingClient.ReplyMessageAsync(ev.ReplyToken,
                        "You chose the date-time: " + ev.Postback.Params.DateTime);
                    break;
                default:
                    await messagingClient.ReplyMessageAsync(ev.ReplyToken,
                        "Your postback is " + ev.Postback.Data);
                    break;
            }
        }

        protected override async Task OnFollowAsync(FollowEvent ev)
        {
            throw new NotImplementedException();
        }

        protected override async Task OnUnfollowAsync(UnfollowEvent ev)
        {
            throw new NotImplementedException();
        }

        protected override async Task OnJoinAsync(JoinEvent ev)
        {
            await messagingClient.ReplyMessageAsync(ev.ReplyToken, $"Thank you for letting me join your {ev.Source.Type.ToString().ToLower()}!");
        }

        protected override async Task OnLeaveAsync(LeaveEvent ev)
        {
            throw new NotImplementedException();
        }

        protected override async Task OnBeaconAsync(BeaconEvent ev)
        {
            var message = "";
            switch (ev.Beacon.Type)
            {
                case BeaconType.Enter:
                    message = "You entered the beacon area!";
                    break;
                case BeaconType.Leave:
                    message = "You leaved the beacon area!";
                    break;
                case BeaconType.Banner:
                    message = "You tapped the beacon banner!";
                    break;
            }

            await messagingClient.ReplyMessageAsync(ev.ReplyToken, $"{message}(Dm:{ev.Beacon.Dm}, Hwid:{ev.Beacon.Hwid})");
        }

        #endregion

        private async Task HandleTextAsync(string replyToken, string userMessage, string userId)
        {
            userMessage = userMessage.ToLower().Replace(" ", "");
            ISendMessage replyMessage = null;
            if (userMessage == "buttons")
            {
                replyMessage = new TemplateMessage("Button Template",
                    new ButtonsTemplate(text: "ButtonsTemplate", title: "Click Buttons.",
                    actions: new List<ITemplateAction> {
                        new MessageTemplateAction("Message Label", "sample data"),
                        //new PostbackTemplateAction("Postback Label", "sample data", "sample data"),
                        new PostbackTemplateAction("Postback Label", "sample data"),
                    new UriTemplateAction("Uri Label", "https://github.com/kenakamu")
                    }));
            }
            else if (userMessage == "confirm")
            {
                replyMessage = new TemplateMessage("Confirm Template",
                    new ConfirmTemplate("ConfirmTemplate", new List<ITemplateAction> {
                        new MessageTemplateAction("Yes", "Yes"),
                        new MessageTemplateAction("No", "No")
                    }));
            }
            else if (userMessage == "carousel")
            {
                List<ITemplateAction> actions1 = new List<ITemplateAction>();
                List<ITemplateAction> actions2 = new List<ITemplateAction>();

                // Add actions.
                actions1.Add(new MessageTemplateAction("Message Label", "sample data"));
                actions1.Add(new PostbackTemplateAction("Postback Label", "sample data", "sample data"));
                actions1.Add(new UriTemplateAction("Uri Label", "https://github.com/kenakamu"));

                // Add datetime picker actions
                actions2.Add(new DateTimePickerTemplateAction("DateTime Picker", "DateTime",
                    DateTimePickerMode.Datetime, "2017-07-21T13:00", null, null));
                actions2.Add(new DateTimePickerTemplateAction("Date Picker", "Date",
                    DateTimePickerMode.Date, "2017-07-21", null, null));
                actions2.Add(new DateTimePickerTemplateAction("Time Picker", "Time",
                    DateTimePickerMode.Time, "13:00", null, null));

                replyMessage = new TemplateMessage("Button Template",
                    new CarouselTemplate(new List<CarouselColumn> {
                        new CarouselColumn("Casousel 1 Text", "https://github.com/apple-touch-icon.png",
                        "Casousel 1 Title", actions1),
                        new CarouselColumn("Casousel 1 Text", "https://github.com/apple-touch-icon.png",
                        "Casousel 1 Title", actions2)
                    }));
            }
            else if (userMessage == "imagecarousel")
            {
                UriTemplateAction action = new UriTemplateAction("Uri Label", "https://github.com/namhoaingo");

                replyMessage = new TemplateMessage("ImageCarouselTemplate",
                    new ImageCarouselTemplate(new List<ImageCarouselColumn> {
                        new ImageCarouselColumn("https://github.com/apple-touch-icon.png", action),
                        new ImageCarouselColumn("https://github.com/apple-touch-icon.png", action),
                        new ImageCarouselColumn("https://github.com/apple-touch-icon.png", action),
                        new ImageCarouselColumn("https://github.com/apple-touch-icon.png", action),
                        new ImageCarouselColumn("https://github.com/apple-touch-icon.png", action)
                    }));
            }
            else if (userMessage == "imagemap")
            {
                replyMessage = new ImagemapMessage("https://d412efff.ngrok.io/api/image/DeathStar2",
                                                "Image has some problem to load",
                                                new ImagemapSize(1040, 1040),
                                                new List<IImagemapAction> {
                                                        new UriImagemapAction(
                                                            new ImagemapArea(0,0,520,1040), "https://24aa4e3a.ngrok.io/account/Signin/" + userId),
                                                        new MessageImagemapAction(
                                                            new ImagemapArea(520,0,520,1040), "https://www.diffnow.com/"),
                                                            }
                    );
            }
            else if (userMessage == "addrichmenu")
            {
                throw new NotImplementedException();
            }
            else if (userMessage == "deleterichmenu")
            {
                // Get Rich Menu for the user
                var richMenuId = await messagingClient.GetRichMenuIdOfUserAsync(userId);
                await messagingClient.UnLinkRichMenuFromUserAsync(userId);
                await messagingClient.DeleteRichMenuAsync(richMenuId);
                replyMessage = new TextMessage("Rich menu deleted");
            }
            else if (userMessage == "deleteallrichmenu")
            {
                // Get Rich Menu for the user
                var richMenuList = await messagingClient.GetRichMenuListAsync();
                foreach (var richMenu in richMenuList)
                {
                    await messagingClient.DeleteRichMenuAsync(richMenu.RichMenuId);
                }
                replyMessage = new TextMessage("All rich menu added");
            }
            else if (userMessage == "hello,world")
            {
                return;
            }
            else
            {
                //replyMessage = new TextMessage("https://d412efff.ngrok.io/Assets/DeathStar2.jpg");
                return;
            }

            await messagingClient.ReplyMessageAsync(replyToken, new List<ISendMessage> { replyMessage });
        }

        /// <summary>
        /// Upload the received data to blob and returns the address
        /// </summary>
        private async Task HandleMediaAsync(string replyToken, string messageId, string blobDirectoryName, string blobName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reply the location user send.
        /// </summary>
        private async Task HandleLocationAsync(string replyToken, LocationEventMessage location)
        {
            await messagingClient.ReplyMessageAsync(replyToken, new[] {
                        new LocationMessage("Location", location.Address, location.Latitude, location.Longitude)
                    });
        }

        /// <summary>
        /// Replies random sticker
        /// Sticker ID of bssic stickers (packge ID =1)
        /// see https://devdocs.line.me/files/sticker_list.pdf
        /// </summary>
        private async Task HandleStickerAsync(string replyToken)
        {
            var stickerids = Enumerable.Range(1, 17)
                .Concat(Enumerable.Range(21, 1))
                .Concat(Enumerable.Range(100, 139 - 100 + 1))
                .Concat(Enumerable.Range(401, 430 - 400 + 1)).ToArray();

            var rand = new Random(Guid.NewGuid().GetHashCode());
            var stickerId = stickerids[rand.Next(stickerids.Length - 1)].ToString();
            await messagingClient.ReplyMessageAsync(replyToken, new[] {
                        new StickerMessage("1", stickerId)
                    });
        }

        private string GetFileExtension(string mediaType)
        {
            switch (mediaType)
            {
                case "image/jpeg":
                    return ".jpeg";
                case "audio/x-m4a":
                    return ".m4a";
                case "video/mp4":
                    return ".mp4";
                default:
                    return "";
            }
        }
    }
}
