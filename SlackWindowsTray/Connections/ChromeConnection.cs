using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SlackWindowsTray
{
    public class ChromeConnection : WebSocketBehavior
    {

        public static readonly ChromeConnection Instance = new ChromeConnection();
        private ChromeConnection()
        {

        }


        private WebSocketServer _wssv = new WebSocketServer(4649);
        
        protected override void OnOpen()
        {
            OnSlackStateChanged(this, new SlackState(TrayStates.AllRead));
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            dynamic message = JsonConvert.DeserializeObject(e.Data);
            if (message.command == "chat-state")
            {
                var bodyString = JsonConvert.SerializeObject(message.body);
                var chatStateList = JsonConvert.DeserializeObject<List<ChatState>>(bodyString);
                SlackState slackState = new SlackState(chatStateList);
                OnSlackStateChanged(this, slackState);
            }
            else if (message.command == "version")
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;

                message.body = string.Format("{0}.{1}", version.Major, version.Minor);
                this.Send(JsonConvert.SerializeObject(message));
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            OnSlackStateChanged(this, new SlackState(TrayStates.DisconnectedFromExtension));
        }

        public void Start()
        {
            _wssv.AddWebSocketService("/Slack", () => this);
            _wssv.Start();

            if (_wssv.IsListening)
            {
                global::SlackWindowsTray.Log.Write(string.Format("Listening on port {0}, and providing WebSocket services:", _wssv.Port));
                foreach (var path in _wssv.WebSocketServices.Paths)
                {
                    global::SlackWindowsTray.Log.Write(string.Format("- {0}", path));
                }
            }
        }

        public event EventHandler<SlackState> OnSlackStateChanged = delegate { };

    }
}