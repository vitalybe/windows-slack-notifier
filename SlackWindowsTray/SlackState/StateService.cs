﻿using System;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace SlackWindowsTray
{
    class StateService
    {
        private WebSocketServer _wssv = new WebSocketServer(4649);
        private StateProcessorsChain _processorsChain;
        private SlackNotifierStates _lastSlackState;

        private StateService()
        {
            _processorsChain = new StateProcessorsChain();
            _processorsChain.AddProcessor(new StateCallbackProcessor(state => OnStateChange(null, state)));
            _processorsChain.AddProcessor(new StateAnimationProcessor());

            _processorsChain.HandleState(SlackNotifierStates.DisconnectedFromExtension);

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

            _processorsChain.HandleState(state);
        }

        public static readonly StateService Instance = new StateService();

        public event EventHandler<SlackNotifierStates> OnStateChange = delegate { };

        public async Task Snooze()
        {
            var snoozingProcessor = new SnoozingProcessor(_lastSlackState);
            _processorsChain.AddProcessor(snoozingProcessor);
            await Task.Delay(TimeSpan.FromSeconds(3));
            _processorsChain.RemoveProcessor(snoozingProcessor);
        }
    }
}