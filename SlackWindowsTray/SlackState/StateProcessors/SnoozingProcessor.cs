using System.Collections.Generic;

namespace SlackWindowsTray
{
    class SnoozingProcessor : StateProcessorBase
    {
        private SlackState _lastSlackState;

        public SnoozingProcessor(SlackState slackState)
        {
            _lastSlackState = slackState;
        }

        public override StateProcessorPriorityEnum Priority
        {
            get { return StateProcessorPriorityEnum.Snoozing; }
        }

        public override void OnAdd()
        {
            NextHandleState(new SlackState(TrayStates.AllRead));
        }

        public override void OnRemove()
        {
            NextHandleState(_lastSlackState);
        }

        protected override bool HandleStateRaw(SlackState slackState)
        {
            _lastSlackState = slackState;

            return false;
        }
    }
}