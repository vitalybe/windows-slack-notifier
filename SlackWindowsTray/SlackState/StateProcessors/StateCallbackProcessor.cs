using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SlackWindowsTray
{
    class StateCallbackProcessor : StateProcessorBase
    {
        private readonly Action<TrayStates> _callbackAction;


        public StateCallbackProcessor(Action<TrayStates> callbackAction)
        {
            _callbackAction = callbackAction;
        }

        protected override bool HandleStateRaw(SlackState slackState)
        {
            _callbackAction(slackState.TrayState);

            return true;
        }

        public override StateProcessorPriorityEnum Priority
        {
            get
            {
                return StateProcessorPriorityEnum.Callback;
            }
        }
    }
}