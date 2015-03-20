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
    // Responsible for connecting to Slack RTM, maintaining the connection and creating events for appropriate listeners
    class SlackRtm
    {
        private WebSocket _webSocket = null;

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
            var rtmInfo = Utils.Instance.SlackApiCall("rtm.start");
            ConnectWebSocket(rtmInfo.url.Value);
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
                Log.Write("Recieved message: " + e.Data);

                RtmMessageService.Instance.OnMessage(message);
            }
        }
    }
}