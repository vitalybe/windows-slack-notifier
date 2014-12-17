namespace SlackWindowsTray
{
    class SnoozingProcessor : StateProcessorBase
    {
        private SlackNotifierStates _lastState;
        private SlackNotifierStates _lastSlackState;

        public SnoozingProcessor(SlackNotifierStates _lastSlackState)
        {
            _lastState = _lastSlackState;
        }

        protected override StateProcessorPriorityEnum Priority
        {
            get { return StateProcessorPriorityEnum.Snoozing; }
        }

        protected override void OnAdd()
        {
            NextHandleState(SlackNotifierStates.AllRead);
        }

        protected override void OnRemove()
        {
            NextHandleState(_lastState);
        }

        protected override bool HandleStateRaw(SlackNotifierStates state)
        {
            _lastState = state;

            return false;
        }
    }
}