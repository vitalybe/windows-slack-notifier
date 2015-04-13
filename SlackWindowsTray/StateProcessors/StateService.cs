using System;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace SlackWindowsTray
{
    class StateService
    {
        private SnoozingProcessor _snoozingProcessor;
        private StateProcessorsChain _processorsChain;
        private SlackState _lastSlackState;
        private ChromeConnection _chromeConnection = ChromeConnection.Instance;

        private StateService()
        {
        }

        public void Start()
        {
            _snoozingProcessor = new SnoozingProcessor(() => OnSnoozeFinished(this, null));

            _processorsChain = new StateProcessorsChain();
            _processorsChain.AddProcessor(new StateCallbackProcessor(slackState => OnStateChange(null, slackState)));
            _processorsChain.AddProcessor(new StateAnimationProcessor());
            _processorsChain.AddProcessor(_snoozingProcessor);

            ConnectionToExtension();
        }

        private void ConnectionToExtension()
        {
            _chromeConnection.Start();
            _chromeConnection.OnSlackStateChanged += (o, state) => UpdateState(state);
        }

        private void UpdateState(SlackState state)
        {
            _lastSlackState = state;

            _processorsChain.HandleState(state);
        }

        public static readonly StateService Instance = new StateService();

        public event EventHandler<SlackState> OnStateChange = delegate { };
        public event EventHandler OnSnoozeFinished = delegate { };

        public void Snooze(string chatName = null)
        {
            _snoozingProcessor.Snooze(chatName);
        }

        public void Unsnooze()
        {
            _snoozingProcessor.Unsnooze();
        }

        public void Refresh()
        {
            UpdateState(_lastSlackState);
        }
    }
}