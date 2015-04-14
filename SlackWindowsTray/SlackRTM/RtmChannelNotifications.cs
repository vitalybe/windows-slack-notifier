using System;
using System.Collections.Generic;
using System.Linq;
using ToastNotifications;

namespace SlackWindowsTray.SlackRTM
{
    class RtmChannelNotifications
    {
        private SnoozingService _snoozingService = SnoozingService.Instance;

        private readonly Dictionary<string, List<IncomingMessage>> _snoozedMessages = new Dictionary<string, List<IncomingMessage>>();
        private readonly Dictionary<string, Notification> _channels = new Dictionary<string, Notification>();

        private void ShowAllSnoozed(string channelName)
        {
            foreach (var incomingMessage in _snoozedMessages[channelName])
            {
                ShowNotification(incomingMessage);
            }

            _snoozedMessages[channelName].Clear();
        }

        private void SnoozingServiceOnOnChannelSnoozeFinished(object sender, string channelName)
        {
            if (channelName != null)
            {
                if (_snoozedMessages.ContainsKey(channelName))
                {
                    ShowAllSnoozed(channelName);
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

        private void SnoozingServiceOnOnChannelSnooze(object sender, string channelName)
        {
            if (channelName != null)
            {
                if (_channels.ContainsKey(channelName))
                {
                    _channels[channelName].Close();
                }
            }
            else
            {
                foreach (var openChannel in _channels.Values)
                {
                    openChannel.Close();
                }
            }
        }


        public void ShowNotification(IncomingMessage message)
        {
            if (_snoozingService.IsDndMode || _snoozingService.IsChannelSnoozed(message.ChannelName))
            {
                if (!_snoozedMessages.ContainsKey(message.ChannelName))
                {
                    _snoozedMessages[message.ChannelName] = new List<IncomingMessage>();
                }

                _snoozedMessages[message.ChannelName].Add(message);
            }
            else
            {
                if (MainWindow.Form.InvokeRequired)
                {
                    MainWindow.Form.Invoke((Action)(delegate { ShowNotification(message); }));
                    return;
                }

                if (!_channels.ContainsKey(message.ChannelName))
                {
                    var newNotification = new Notification(message.ChannelId, message.ChannelName, 20, FormAnimator.AnimationMethod.Slide, FormAnimator.AnimationDirection.Up);
                    newNotification.Closed += OnChannelClosed;
                    newNotification.OnQuickReply += OnChannelQuickReply;

                    newNotification.Show();
                    _channels.Add(message.ChannelName, newNotification);
                }

                Notification toastNotification = _channels[message.ChannelName];
                toastNotification.AddMessage(message.User, message.MessageText, message.IsIncoming);
            }
        }

        private void OnChannelQuickReply(object sender, string reply)
        {
            var notification = (Notification)sender;
            SlackApi.Instance.PostMessage(notification.ChannelId, reply);
        }

        private void OnChannelClosed(object sender, EventArgs eventArgs)
        {
            var notification = (Notification) sender;
            _channels.Remove(notification.ChannelName);
        }


        public static readonly RtmChannelNotifications Instance = new RtmChannelNotifications();
        private RtmChannelNotifications()
        {
            _snoozingService.OnChannelSnooze += SnoozingServiceOnOnChannelSnooze;
            _snoozingService.OnChannelSnoozeFinished += SnoozingServiceOnOnChannelSnoozeFinished;
        }
    }
}
