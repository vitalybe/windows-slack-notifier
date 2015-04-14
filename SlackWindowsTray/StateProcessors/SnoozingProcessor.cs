using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SlackWindowsTray
{
    // Can snooze either all notifications
    // Or unread notifications from specific channels (won't snooze mentions)
    class SnoozingProcessor : StateProcessorBase
    {
        private SnoozingService _snoozingService = SnoozingService.Instance;
        private SlackState _lastSlackState = null;
        private readonly Dictionary<string, ChatState> _snoozedChannelStates = new Dictionary<string, ChatState>();

        private void SnoozeChat(SlackState lastSlackState, string chatName)
        {
            var snoozedChatState = new ChatState {name = chatName, unread = false, mention = false};
            var originalChatState = lastSlackState.ReplaceChatState(snoozedChatState);
            _snoozedChannelStates[chatName] = originalChatState;
        }

        private void SnoozingServiceOnOnChannelSnooze(object sender, string channelName)
        {
            if (!string.IsNullOrEmpty(channelName))
            {
                SnoozeChat(_lastSlackState, channelName);
                NextHandleState(_lastSlackState);
            }
            else
            {
                NextHandleState(new SlackState(TrayStates.AllRead));
            }

        }

        private void SnoozingServiceOnOnChannelSnoozeFinished(object sender, string channelName)
        {
            if (!string.IsNullOrEmpty(channelName))
            {
                _lastSlackState.ReplaceChatState(_snoozedChannelStates[channelName]);
                _snoozedChannelStates.Remove(channelName);
                NextHandleState(_lastSlackState);
            }
            else
            {
                NextHandleState(_lastSlackState);
            }

        }

        protected override bool HandleStateRaw(SlackState slackState)
        {
            _lastSlackState = slackState;

            if (_snoozingService.IsDndMode)
            {
                // Don't proceed with the state change if everything is snoozed - We don't want to show anything to the user anyway
                return false;
            }
            else
            {
                List<string> channelNames = slackState.ChatStates.Select(chatState => chatState.name).ToList();
                foreach (var channelName in channelNames)
                {
                    if (_snoozingService.IsChannelSnoozed(channelName))
                    {
                        SnoozeChat(slackState, channelName);
                    }
                }

                return true;
            }
        }

        public SnoozingProcessor()
        {
            _snoozingService.OnChannelSnooze += SnoozingServiceOnOnChannelSnooze;
            _snoozingService.OnChannelSnoozeFinished += SnoozingServiceOnOnChannelSnoozeFinished;
        }
    }
}