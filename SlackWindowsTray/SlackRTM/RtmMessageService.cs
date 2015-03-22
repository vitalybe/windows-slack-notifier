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
    class RtmMessageService
    {
        // Channels, groups, users - ID and name
        private Dictionary<string, string> _slackObjects = new Dictionary<string, string>();
        private bool _isRefreshing = false;

        private string SlackIdToName(string id)
        {
            if (!_slackObjects.ContainsKey(id))
            {
                RefreshAll();
            }

            var name = "UNKNOWN";
            if (_slackObjects.ContainsKey(id))
            {
                name = _slackObjects[id];
            }

            return name;
        }

        private void RefreshAll()
        {
            if (_isRefreshing)
            {
                // Prevent recursion
                return;
            }

            _isRefreshing = true;
            try
            {
                _slackObjects.Clear();
                _slackObjects.Add("USLACKBOT", "slackbot");

                RefreshData("channels.list", "channels", "#");
                RefreshData("users.list", "members");
                RefreshData("groups.list", "groups");
                RefreshImData();
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void RefreshData(string api, string collectionName, string prefix = "")
        {
            var dataCollection = Utils.Instance.SlackApiCall(api)[collectionName];
            foreach (dynamic data in dataCollection)
            {
                _slackObjects.Add(data.id.Value, prefix + data.name.Value);
            }
        }

        private void RefreshImData()
        {
            var dataCollection = Utils.Instance.SlackApiCall("im.list")["ims"];
            foreach (dynamic data in dataCollection)
            {
                _slackObjects.Add(data.id.Value, SlackIdToName(data.user.Value));
            }
        }


        public void OnMessage(dynamic message)
        {
            try
            {
                var channelName = SlackIdToName(message.channel.Value);
                var user = SlackIdToName(message.user.Value);

                // Find object names in the message (e.g user references) and relace them
                Regex messageIdRegex = new Regex("<@([A-Z0-9]+)>");
                string messageText = message.text.Value;
                messageText = messageIdRegex.Replace(messageText, match =>
                {
                    var id = match.Groups[1].Value;
                    return SlackIdToName(id);
                });

                Log.Write(string.Format("Parsed message: [{0}] {1}: {2}", channelName, user, messageText));

                RtmChannelNotifications.Instance.ShowNotification(channelName, user, messageText);
            }
            catch (Exception e)
            {
                Log.Write("ERROR: Failed to display message: " + e.Message);
            }
        }

        public static readonly RtmMessageService Instance = new RtmMessageService();
        private RtmMessageService()
        {
        }

    }
}