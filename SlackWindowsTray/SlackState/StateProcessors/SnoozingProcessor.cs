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
        private readonly Action _snoozeFinishedCallback;
        private SlackState _lastSlackState;
        
        private bool _allSnoozed = false;
        private readonly Dictionary<string, ChatState> _snoozedChats = new Dictionary<string, ChatState>();
        private CancellationTokenSource _snoozeCancellationToken = new CancellationTokenSource();

        public SnoozingProcessor(Action snoozeFinishedCallback)
        {
            _snoozeFinishedCallback = snoozeFinishedCallback;
        }

        public override StateProcessorPriorityEnum Priority
        {
            get { return StateProcessorPriorityEnum.Snoozing; }
        }

        public async void Snooze(string chatName = null)
        {
            if (chatName != null)
            {
                SnoozeChat(_lastSlackState, chatName);

                NextHandleState(_lastSlackState);

                try
                {
                    var soozeTimeout = TimeSpan.FromMinutes(SlackWindowsTray.Default.SnoozeChatTimeMinutes);
                    await Task.Delay(soozeTimeout, _snoozeCancellationToken.Token);
                }
                catch (TaskCanceledException e)
                {
                }

                _lastSlackState.ReplaceChatState(_snoozedChats[chatName]);
                _snoozedChats.Remove(chatName);

                NextHandleState(_lastSlackState);

                CallbackIfAllSnoozeFinished();
            }
            else
            {
                _allSnoozed = true;
                NextHandleState(new SlackState(TrayStates.AllRead));

                try
                {
                    var soozeTimeout = TimeSpan.FromMinutes(SlackWindowsTray.Default.SnoozeAllTimeMinutes);
                    await Task.Delay(soozeTimeout, _snoozeCancellationToken.Token);
                }
                catch (TaskCanceledException e)
                {
                }
                
                _allSnoozed = false;
                NextHandleState(_lastSlackState);

                CallbackIfAllSnoozeFinished();
            }
        }

        private void CallbackIfAllSnoozeFinished()
        {
            if (_allSnoozed == false && _snoozedChats.Count == 0)
            {
                _snoozeFinishedCallback();
            }
        }

        private void SnoozeChat(SlackState lastSlackState, string chatName)
        {
            var snoozedChatState = new ChatState {name = chatName};
            var originalChatState = lastSlackState.ReplaceChatState(snoozedChatState);
            _snoozedChats[chatName] = originalChatState;
        }

        protected override bool HandleStateRaw(SlackState slackState)
        {
            _lastSlackState = slackState;

            if (_allSnoozed)
            {
                // Don't proceed with the state change if everything is snoozed - We don't want to show anything to the user anyway
                return false;
            }
            else
            {
                foreach (var chatName in _snoozedChats.Keys)
                {
                    SnoozeChat(_lastSlackState, chatName);
                }

                return true;
            }
        }

        public void Unsnooze()
        {
            _snoozeCancellationToken.Cancel();
            _snoozeCancellationToken = new CancellationTokenSource();
        }
    }
}