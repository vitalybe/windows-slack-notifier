using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyHttp.Http;
using Newtonsoft.Json;

namespace SlackWindowsTray
{
    class Utils
    {
        public static readonly Utils Instance = new Utils();
        private Utils()
        {
        }

        public dynamic SlackApiCall(string apiName)
        {
            var url = string.Format("https://slack.com/api/{0}?token={1}", apiName, SlackWindowsTray.Default.SlackToken);

            var http = new HttpClient();
            var response = http.Get(url);
            return JsonConvert.DeserializeObject(response.RawText);
        }


    }
}
