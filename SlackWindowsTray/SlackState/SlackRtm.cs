using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyHttp.Http;
using Newtonsoft.Json;
using ToastNotifications;
using WebSocketSharp;

namespace SlackWindowsTray
{
    class SlackRtm
    {
        private WebSocket _webSocket = null;
        // Channels, groups, users - ID and name
        private Dictionary<string, string> _slackObjects = new Dictionary<string, string>();

        public static readonly SlackRtm Instance = new SlackRtm();
        private SlackRtm()
        {
        }

        public void Start(Form owner)
        {
            _owner = owner;
            Task.Factory.StartNew(ConnectRtm);
        }

        private void ConnectRtm()
        {
            var rtmInfo = SlackApiCall("rtm.start");
            ConnectWebSocket(rtmInfo.url.Value);
        }

        private static dynamic SlackApiCall(string apiName)
        {
            var url = string.Format("https://slack.com/api/{0}?token={1}", apiName, SlackWindowsTray.Default.SlackToken);

            var http = new HttpClient();
            var response = http.Get(url);
            return JsonConvert.DeserializeObject(response.RawText);
        }

        private void ConnectWebSocket(string url)
        {
            _webSocket = new WebSocket(url);
            _webSocket.OnMessage += OnSocketMessage;
            _webSocket.OnClose += (sender, e) => Console.WriteLine("Connection closed: " + e.Reason);
            _webSocket.Connect ();
        }

        private void OnSocketMessage(object sender, MessageEventArgs e)
        {
            dynamic message = JsonConvert.DeserializeObject(e.Data);
            if (message.type == "message")
            {
                var channelName = SlackIdToName(message.channel.Value);
                var user = SlackIdToName(message.user.Value);
                
                Regex messageIdRegex = new Regex("<@([A-Z0-9]+)>");
                string text = message.text.Value;
                text = messageIdRegex.Replace(text, match =>
                {
                    var id = match.Groups[1].Value;
                    return SlackIdToName(id);
                });

                _owner.UIThread(delegate()
                {
                    var toastNotification = new Notification(channelName, string.Format("{0}: {1}", user, text),
                        -1, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up);
                    toastNotification.Show();
                });

                Console.WriteLine("[{0}] {1}: {2}", channelName, user, text);
            }
        }

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

        private bool isRefreshing = false;
        private Form _owner;

        private void RefreshAll()
        {
            if (isRefreshing)
            {
                // Prevent recursion
                return;
            }

            isRefreshing = true;
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
                isRefreshing = false;
            }
        }

        private void RefreshData(string api, string collectionName, string prefix = "")
        {
            var dataCollection = SlackApiCall(api)[collectionName];
            foreach (dynamic data in dataCollection)
            {
                _slackObjects.Add(data.id.Value, prefix + data.name.Value);
            }
        }

        private void RefreshImData()
        {
            var dataCollection = SlackApiCall("im.list")["ims"];
            foreach (dynamic data in dataCollection)
            {
                _slackObjects.Add(data.id.Value, SlackIdToName(data.user.Value));
            }
        }

    }
}