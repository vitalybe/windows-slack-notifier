using System;
using System.Collections.Generic;
using ToastNotifications;

namespace SlackWindowsTray.SlackRTM
{
    class RtmChannelNotifications
    {
        private Dictionary<string, Notification> _channels = new Dictionary<string, Notification>();

        public void ShowNotification(string channelId, string channelName, string user, string message)
        {
            if (MainWindow.Form.InvokeRequired)
            {
                MainWindow.Form.Invoke((Action)(delegate { ShowNotification(channelId, channelName, user, message); }));
                return;
            }

            if (!_channels.ContainsKey(channelId))
            {
                var newNotification = new Notification(channelId, channelName, -1, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up);
                newNotification.Closed += ChannelClosed;
                newNotification.OnQuickReply += ChannelQuickReply;

                newNotification.Show();
                _channels.Add(channelId, newNotification);
            }

            Notification toastNotification = _channels[channelId];
            toastNotification.AddMessage(string.Format("{0}: {1}", user, message));
        }

        private void ChannelQuickReply(object sender, string reply)
        {
            var notification = (Notification)sender;
            SlackApi.Instance.PostMessage(notification.ChannelId, reply);
        }

        private void ChannelClosed(object sender, EventArgs eventArgs)
        {
            var notification = (Notification) sender;
            _channels.Remove(notification.ChannelId);
        }


        public static readonly RtmChannelNotifications Instance = new RtmChannelNotifications();
        private RtmChannelNotifications()
        {
        }

    }
}
