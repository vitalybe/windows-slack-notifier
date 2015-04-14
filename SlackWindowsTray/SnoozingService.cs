using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SlackWindowsTray
{
    class SnoozingService
    {
        private readonly HashSet<string> _snoozedChats = new HashSet<string>();
        private bool _isDndMode = false;

        private CancellationTokenSource _snoozeCancellationToken = new CancellationTokenSource();

        // Singletone
        public static readonly SnoozingService Instance = new SnoozingService();
        private SnoozingService()
        {

        }

        /// <summary>
        /// Fires an event when a channel is snoozed
        /// Event parameter is the name of the channel. If all channels are snoozed, the name is null
        /// </summary>
        public event EventHandler<string> OnChannelSnooze = delegate { };

        /// <summary>
        /// Fires an event when a channel is no longer snoozed
        /// Event parameter is the name of the channel. If all channels are no longer snoozed, the name is null
        /// </summary>
        public event EventHandler<string> OnChannelSnoozeFinished = delegate { };

        public bool IsDndMode { get { return _isDndMode; } }
        public bool IsChannelSnoozed(string channel)
        {
            if (channel == null) throw new ArgumentNullException("channel");

            return _snoozedChats.Contains(channel);
        }
        public bool IsAnySnoozeActive { get { return IsDndMode || _snoozedChats.Count > 0; } }

        /// <summary>
        /// Snoozes the given channel.
        /// </summary>
        /// <param name="channel">If all channels are snoozed, the name is null</param>
        public async void Snooze(string channel)
        {
            int snoozeMinutes;
            if (!string.IsNullOrEmpty(channel))
            {
                snoozeMinutes = SlackWindowsTray.Default.SnoozeChatTimeMinutes;
                _snoozedChats.Add(channel);
            }
            else
            {
                snoozeMinutes = SlackWindowsTray.Default.SnoozeAllTimeMinutes;
                _isDndMode = true;
            }

            OnChannelSnooze(this, channel);

            try
            {
                await Task.Delay(TimeSpan.FromMinutes(snoozeMinutes), _snoozeCancellationToken.Token);
            }
            catch (TaskCanceledException e)
            {
                // Everything was unsnoozed
            }

            if (!string.IsNullOrEmpty(channel))
            {
                _snoozedChats.Remove(channel);
            }
            else
            {
                _isDndMode = false;
            }

            OnChannelSnoozeFinished(this, channel);
        }

        /// <summary>
        /// Unsnoozes all channels AND the global snooze if such was set
        /// </summary>
        public void UnsnoozeAll()
        {
            _snoozeCancellationToken.Cancel();
            _snoozeCancellationToken = new CancellationTokenSource();
        }

    }
}