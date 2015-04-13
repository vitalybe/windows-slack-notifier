using System;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace SlackWindowsTray
{
    class StateService
    {
        private WebSocketServer _wssv = new WebSocketServer(4649);
        private SnoozingProcessor _snoozingProcessor;
        private StateProcessorsChain _processorsChain;
        private SlackState _lastSlackState;

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
            _wssv.AddWebSocketService<ChromeConnection>("/Slack");
            ChromeConnection.OnSlackStateChanged += (o, state) => UpdateState(state);

            _wssv.Start();
            if (_wssv.IsListening)
            {
                Log.Write(string.Format("Listening on port {0}, and providing WebSocket services:", _wssv.Port));
                foreach (var path in _wssv.WebSocketServices.Paths)
                {
                    Log.Write(string.Format("- {0}", path));
                }
            }
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