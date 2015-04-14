using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SlackWindowsTray
{
    class StateCallbackProcessor : StateProcessorBase
    {
        private readonly Action<SlackState> _callbackAction;


        public StateCallbackProcessor(Action<SlackState> callbackAction)
        {
            _callbackAction = callbackAction;
        }

        protected override bool HandleStateRaw(SlackState slackState)
        {
            _callbackAction(slackState);

            return true;
        }
    }
}