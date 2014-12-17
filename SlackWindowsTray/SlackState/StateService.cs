using System;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace SlackWindowsTray
{
    class StateService
    {
        private WebSocketServer _wssv = new WebSocketServer(4649);
        private StateProcessorBase _stateProcessor;
        private SlackNotifierStates _lastSlackState;

        private StateService()
        {
            _stateProcessor = new StartProcessor();
            _stateProcessor.AddProcessor(new StateCallbackProcessor(state => OnStateChange(null, state)));
            _stateProcessor.AddProcessor(new StateAnimationProcessor());

            _stateProcessor.HandleState(SlackNotifierStates.DisconnectedFromExtension);

            ConnectionToExtension();
        }

        private void ConnectionToExtension()
        {
            _wssv.AddWebSocketService<SlackEndpoint>("/Slack");
            SlackEndpoint.OnSlackStateChanged += (o, state) => UpdateState(state);

            _wssv.Start();
            if (_wssv.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", _wssv.Port);
                foreach (var path in _wssv.WebSocketServices.Paths)
                {
                    Console.WriteLine("- {0}", path);
                }
            }
        }

        private void UpdateState(SlackNotifierStates state)
        {
            _lastSlackState = state;

            _stateProcessor.HandleState(state);
        }

        public static readonly StateService Instance = new StateService();

        public event EventHandler<SlackNotifierStates> OnStateChange = delegate { };

        public async Task Snooze()
        {
            var snoozingProcessor = new SnoozingProcessor(_lastSlackState);
            _stateProcessor.AddProcessor(snoozingProcessor);
            await Task.Delay(TimeSpan.FromSeconds(10));
            _stateProcessor.RemoveProcessor(snoozingProcessor);
        }
    }
}