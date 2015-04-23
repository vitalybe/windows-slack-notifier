using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EasyHttp.Http;
using Newtonsoft.Json;

namespace SlackWindowsTray
{
    class SlackApi
    {
        public static readonly SlackApi Instance = new SlackApi();
        private SlackApi()
        {
        }

        // Channels, groups, users - ID and name
        private readonly Dictionary<string, string> _slackObjects = new Dictionary<string, string>();
        
        // Holds the connection between IM channel and a user
        private readonly Dictionary<string, string> _imToUserDictionary = new Dictionary<string, string>();
        private bool _isRefreshing = false;

        private void RefreshAll()
        {
            // TODO: Lock multi-thread access
            if (_isRefreshing)
            {
                // Prevent recursion
                return;
            }

            _isRefreshing = true;
            try
            {
                _imToUserDictionary.Clear();
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
            var dataCollection = SlackApi.Instance.Get(api)[collectionName];
            foreach (dynamic data in dataCollection)
            {
                _slackObjects.Add(data.id.Value, prefix + data.name.Value);
            }
        }

        private void RefreshImData()
        {
            var dataCollection = SlackApi.Instance.Get("im.list")["ims"];
            foreach (dynamic data in dataCollection)
            {
                _slackObjects.Add(data.id.Value, SlackIdToName(data.user.Value));
                _imToUserDictionary.Add(data.id.Value, data.user.Value);
            }
        }

        /// <summary>
        /// Converts between a im channel id and its user id, e.g: D838123 -> U72312
        /// </summary>
        public string SlackImToUser(string imChannelId)
        {
            if (!_imToUserDictionary.ContainsKey(imChannelId))
            {
                RefreshAll();
            }

            var userId = "UNKNOWN_USER";
            if (_imToUserDictionary.ContainsKey(imChannelId))
            {
                userId = _imToUserDictionary[imChannelId];
            }

            return userId;
        }
        public string SlackIdToName(string id)
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

        public dynamic Get(string apiName)
        {
            var url = string.Format("https://slack.com/api/{0}?token={1}", apiName, SlackWindowsTray.Default.SlackToken);

            var http = new HttpClient();
            var response = http.Get(url);
            return JsonConvert.DeserializeObject(response.RawText);
        }

        public void PostMessage(string channelId, string text)
        {
            var url = string.Format("https://slack.com/api/chat.postMessage?token={0}&channel={1}&as_user=true", SlackWindowsTray.Default.SlackToken, channelId);

            dynamic data = new ExpandoObject(); // Or any dynamic type
            data.text = text;

            var http = new HttpClient();
            var response = http.Post(url, data, HttpContentTypes.ApplicationXWwwFormUrlEncoded).DynamicBody;
            if (!response.ok)
            {
                throw new ExternalException("Message failed: " + response);
            }
        }
    }
}
