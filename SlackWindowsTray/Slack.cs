using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SlackWindowsTray
{
    public class Slack : WebSocketBehavior
    {

        protected override void OnOpen()
        {
            System.Windows.Forms.MessageBox.Show("Open");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Message: " + e.Data);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Close");
        }
    }
}