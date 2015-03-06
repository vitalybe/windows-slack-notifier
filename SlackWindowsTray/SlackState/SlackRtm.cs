using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using EasyHttp.Http;
using Newtonsoft.Json;
using WebSocketSharp;

namespace SlackWindowsTray
{
    class SlackRtm
    {
        private WebSocket _webSocket = null;
        private Dictionary<string, string> _channels = new Dictionary<string, string>();
        private Dictionary<string, string> _users = new Dictionary<string, string>();
        private Dictionary<string, string> _groups = new Dictionary<string, string>();

        public static readonly SlackRtm Instance = new SlackRtm();
        private SlackRtm()
        {
        }

        public void Start()
        {
            Task.Factory.StartNew(ConnectRtm);
        }

        private void ConnectRtm()
        {
            var rtmInfo = StartRtm();
            ConnectWebSocket(rtmInfo.url.Value);
        }


        private static dynamic StartRtm()
        {
            var url = string.Format("https://slack.com/api/rtm.start?token={0}", SlackWindowsTray.Default.SlackToken);

            var http = new HttpClient();
            var response = http.Get(url);
            return JsonConvert.DeserializeObject(response.RawText);
        }

        private void ConnectWebSocket(string url)
        {
            _webSocket = new WebSocket(url);
            _webSocket.OnMessage += (sender, e) => Console.WriteLine ("Laputa says: " + e.Data);
            _webSocket.OnClose += (sender, e) => Console.WriteLine("Connection closed: " + e.Reason);
            _webSocket.Connect ();
        }
    }
}