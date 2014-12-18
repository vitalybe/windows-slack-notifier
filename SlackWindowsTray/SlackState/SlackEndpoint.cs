using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SlackWindowsTray
{
    public class SlackEndpoint : WebSocketBehavior
    {
        public static event EventHandler<SlackState> OnSlackStateChanged = delegate { };

        protected override void OnOpen()
        {
            OnSlackStateChanged(this, new SlackState(TrayStates.AllRead));
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var chatStateList = JsonConvert.DeserializeObject<List<ChatState>>(e.Data);
            SlackState slackState = new SlackState(chatStateList);

            OnSlackStateChanged(this, slackState);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            OnSlackStateChanged(this, new SlackState(TrayStates.DisconnectedFromExtension));
        }
    }
}