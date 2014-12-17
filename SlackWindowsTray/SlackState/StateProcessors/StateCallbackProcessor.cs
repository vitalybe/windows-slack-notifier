using System;

namespace SlackWindowsTray
{
    class StateCallbackProcessor : StateProcessorBase
    {
        private readonly Action<SlackNotifierStates> _callbackAction;


        public StateCallbackProcessor(Action<SlackNotifierStates> callbackAction)
        {
            _callbackAction = callbackAction;
        }

        protected override bool HandleStateRaw(SlackNotifierStates state)
        {
            _callbackAction(state);

            return true;
        }

        protected override StateProcessorPriorityEnum Priority
        {
            get
            {
                return StateProcessorPriorityEnum.Callback;
            }
        }
    }
}