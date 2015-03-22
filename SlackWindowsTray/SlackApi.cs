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
