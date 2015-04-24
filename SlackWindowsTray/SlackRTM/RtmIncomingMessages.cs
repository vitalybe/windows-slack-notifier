using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using EasyHttp.Http;
using Newtonsoft.Json;
using SlackWindowsTray.SlackRTM;
using ToastNotifications;

namespace SlackWindowsTray
{
    // Parses recieved messages
    class RtmIncomingMessages
    {
        private SlackApi _slackApi = SlackApi.Instance;
        private string myUsername = null;

        public void OnMessage(dynamic message)
        {
            try
            {
                if (message.subtype != null && (message.subtype.value == "bot_message" || message.subtype.value == "file_comment"))
                {
                    return;                   
                }

                var channelId = GetChannelId(message);
                var channelName = _slackApi.SlackIdToName(channelId);
                var user = _slackApi.SlackIdToName(message.user.Value);
                bool isIncoming = myUsername != user;

                // Find object names in the message (e.g user references) and relace them
                Regex messageIdRegex = new Regex("<@([A-Z0-9]+)>");
                string messageText = message.text.Value;
                messageText = messageIdRegex.Replace(messageText, match =>
                {
                    var id = match.Groups[1].Value;
                    return _slackApi.SlackIdToName(id);
                });

                Log.Write(string.Format("Parsed message: [{0}] {1}: {2}", channelName, user, messageText));

                var messageModel = new RtmMessageModel
                {
                    ChannelId = channelId,
                    ChannelName = channelName,
                    User = user,
                    MessageText = messageText,
                    IsIncoming = isIncoming
                };
                RtmChannelNotifications.Instance.ProcessMessage(messageModel);
            }
            catch (Exception e)
            {
                Log.Write("ERROR: Failed to display message: " + e.Message);
            }
        }

        private string GetChannelId(dynamic message)
        {
            // RTM IM messages are received via IM id, e.g: D823482. 
            // However, from the Chrome extension they are received as a user ID: U234234
            // The following logic translates it
            string channelId;
            if (message.channel.Value.StartsWith("D"))
            {
                channelId = _slackApi.SlackImToUser(message.channel.Value);
            }
            else
            {
                channelId = message.channel.Value;
            }
            return channelId;
        }

        public static readonly RtmIncomingMessages Instance = new RtmIncomingMessages();
        private RtmIncomingMessages()
        {
            var myInfo = SlackApi.Instance.Get("auth.test");
            myUsername = myInfo.user;

        }

    }
}