using System;
using System.Collections.Generic;
using ToastNotifications;

namespace SlackWindowsTray.SlackRTM
{
    class RtmChannelNotifications
    {
        private Dictionary<string, Notification> _notifications = new Dictionary<string, Notification>();

        public void ShowNotification(string channel, string user, string message)
        {
            if (MainWindow.Form.InvokeRequired)
            {
                MainWindow.Form.Invoke((Action)(delegate { ShowNotification(channel, user, message); }));
                return;
            }

            if (!_notifications.ContainsKey(channel))
            {
                var newNotification = new Notification(channel, -1, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up);
                newNotification.Closed += NotificationClosed;

                newNotification.Show();
                _notifications.Add(channel, newNotification);
            }

            Notification toastNotification = _notifications[channel];
            toastNotification.AddMessage(string.Format("{0}: {1}", user, message));

        }

        private void NotificationClosed(object sender, EventArgs eventArgs)
        {
            var notification = (Notification) sender;
            _notifications.Remove(notification.Channel);
        }


        public static readonly RtmChannelNotifications Instance = new RtmChannelNotifications();
        private RtmChannelNotifications()
        {
        }

    }
}
