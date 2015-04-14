using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace SlackWindowsTray
{
    class StateService
    {
        private ChromeConnection _chromeConnection = ChromeConnection.Instance;
        private StateProcessorsChain _processorsChain = new StateProcessorsChain();
        private SlackState _lastSlackState;
        private SnoozingProcessor _snoozingProcessor;

        private StateService()
        {
            _chromeConnection.Start();
            _chromeConnection.OnSlackStateChanged += (o, state) => UpdateState(state);

            _processorsChain = new StateProcessorsChain();
            _processorsChain.AddProcessor(new SnoozingProcessor());
            _processorsChain.AddProcessor(new StateAnimationProcessor());
            _processorsChain.AddProcessor(new StateCallbackProcessor(slackState => OnStateChange(null, slackState)));
        }

        private void UpdateState(SlackState state)
        {
            _lastSlackState = state;
            _processorsChain.HandleState(state);
        }

        public static readonly StateService Instance = new StateService();

        public event EventHandler<SlackState> OnStateChange = delegate { };

        public void Refresh()
        {
            UpdateState(_lastSlackState);
        }
    }
}