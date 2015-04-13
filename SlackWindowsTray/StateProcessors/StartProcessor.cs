using System.Collections.Generic;

namespace SlackWindowsTray
{
    class StartProcessor : StateProcessorBase
    {
        public override StateProcessorPriorityEnum Priority
        {
            get { return StateProcessorPriorityEnum.Start; }
        }

        protected override bool HandleStateRaw(SlackState slackState)
        {
            return true;
        }
    }
}