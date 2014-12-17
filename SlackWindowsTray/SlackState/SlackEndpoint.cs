using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SlackWindowsTray
{
    public class SlackEndpoint : WebSocketBehavior
    {
        public static event EventHandler<SlackNotifierStates> OnSlackStateChanged = delegate {};

        protected override void OnOpen()
        {
            OnSlackStateChanged(this, SlackNotifierStates.AllRead);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var state = (SlackNotifierStates) Enum.Parse(typeof(SlackNotifierStates), e.Data);
            OnSlackStateChanged(this, state);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            OnSlackStateChanged(this, SlackNotifierStates.DisconnectedFromExtension);
        }
    }
}