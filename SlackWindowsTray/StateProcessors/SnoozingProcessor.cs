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
            var snoozedChatState = new ChatState {Id = chatName, Unread = false, Mention = false};
            var originalChatState = lastSlackState.ReplaceChatState(snoozedChatState);
            _snoozedChannelStates[chatName] = originalChatState;
        }

        private void SnoozingServiceOnOnChannelSnooze(object sender, string channelId)
        {
            if (!string.IsNullOrEmpty(channelId))
            {
                SnoozeChat(_lastSlackState, channelId);
                NextHandleState(_lastSlackState);
            }
            else
            {
                NextHandleState(new SlackState(TrayStates.AllRead));
            }

        }

        private void SnoozingServiceOnOnChannelSnoozeFinished(object sender, string channelId)
        {
            if (!string.IsNullOrEmpty(channelId))
            {
                _lastSlackState.ReplaceChatState(_snoozedChannelStates[channelId]);
                _snoozedChannelStates.Remove(channelId);
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
                List<string> channelIds = slackState.ChatStates.Select(chatState => chatState.Id).ToList();
                foreach (var channelId in channelIds)
                {
                    if (_snoozingService.IsChannelSnoozed(channelId))
                    {
                        SnoozeChat(slackState, channelId);
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