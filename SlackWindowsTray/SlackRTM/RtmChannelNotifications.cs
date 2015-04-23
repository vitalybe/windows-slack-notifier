using System;
using System.Collections.Generic;
using System.Linq;
using ToastNotifications;

namespace SlackWindowsTray.SlackRTM
{
    class RtmChannelNotifications
    {
        private SnoozingService _snoozingService = SnoozingService.Instance;

        // The messages that arrived while the channel was snoozed
        private readonly Dictionary<string, List<IncomingMessage>> _snoozedMessages = new Dictionary<string, List<IncomingMessage>>();
        
        // Channel ID -> Open notification
        private readonly Dictionary<string, Notification> _openNotifications = new Dictionary<string, Notification>();

        private void ShowAllSnoozed(string channelId)
        {
            foreach (var incomingMessage in _snoozedMessages[channelId])
            {
                ShowNotification(incomingMessage);
            }

            _snoozedMessages[channelId].Clear();
        }

        private void OnChannelSnoozeFinished(object sender, string channelId)
        {
            if (channelId != null)
            {
                if (_snoozedMessages.ContainsKey(channelId))
                {
                    ShowAllSnoozed(channelId);
                }
            }
            else
            {
                foreach (var channelWithSnoozedMessages in _snoozedMessages.Keys)
                {
                    if (_snoozingService.IsChannelSnoozed(channelWithSnoozedMessages) == false)
                    {
                        ShowAllSnoozed(channelWithSnoozedMessages);
                    }
                }
            }
        }

        private void OnChannelSnooze(object sender, string channelId)
        {
            if (channelId != null)
            {
                if (_openNotifications.ContainsKey(channelId))
                {
                    _openNotifications[channelId].Close();
                }
            }
            else
            {
                foreach (var openChannel in _openNotifications.Values.ToList())
                {
                    openChannel.Close();
                }
            }
        }

        private void OnNotificationSnoozeClick(object sender, EventArgs e)
        {
            var notification = (Notification)sender;
            _snoozingService.Snooze(notification.ChannelId);
        }

        private void OnChannelQuickReply(object sender, string reply)
        {
            var notification = (Notification)sender;
            SlackApi.Instance.PostMessage(notification.ChannelId, reply);
        }

        private void OnChannelClosed(object sender, EventArgs eventArgs)
        {
            var notification = (Notification)sender;
            _openNotifications.Remove(notification.ChannelId);
        }

        public IEnumerable<string> OpenNotificationsIds { get { return _openNotifications.Keys; } }

        public void ShowNotification(IncomingMessage message)
        {
            if (_snoozingService.IsDndMode || _snoozingService.IsChannelSnoozed(message.ChannelId))
            {
                if (!_snoozedMessages.ContainsKey(message.ChannelId))
                {
                    _snoozedMessages[message.ChannelId] = new List<IncomingMessage>();
                }

                _snoozedMessages[message.ChannelId].Add(message);
            }
            else
            {
                if (MainWindow.Form.InvokeRequired)
                {
                    MainWindow.Form.Invoke((Action)(delegate { ShowNotification(message); }));
                    return;
                }

                if (!_openNotifications.ContainsKey(message.ChannelId))
                {
                    var newNotification = new Notification(message.ChannelId, message.ChannelName, 20, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up);
                    newNotification.Closed += OnChannelClosed;
                    newNotification.OnQuickReply += OnChannelQuickReply;
                    newNotification.OnSnooze += OnNotificationSnoozeClick;

                    newNotification.Show();
                    _openNotifications.Add(message.ChannelId, newNotification);
                }

                Notification toastNotification = _openNotifications[message.ChannelId];
                toastNotification.AddMessage(message.User, message.MessageText, message.IsIncoming);
            }
        }

        public static readonly RtmChannelNotifications Instance = new RtmChannelNotifications();
        private RtmChannelNotifications()
        {
            _snoozingService.OnChannelSnooze += OnChannelSnooze;
            _snoozingService.OnChannelSnoozeFinished += OnChannelSnoozeFinished;
        }
    }
}
