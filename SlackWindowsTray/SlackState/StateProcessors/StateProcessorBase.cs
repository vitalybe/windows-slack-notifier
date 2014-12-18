using System.Collections.Generic;

namespace SlackWindowsTray
{
    abstract class StateProcessorBase
    {
        protected abstract bool HandleStateRaw(SlackState slackState);

        public virtual void OnAdd() { }
        public virtual void OnRemove() { }

        public abstract StateProcessorPriorityEnum Priority { get; }

        public StateProcessorBase Next { get; set; }

        public void HandleState(SlackState slackState)
        {
            HandleState(slackState, skipMyself: false);
        }

        protected void NextHandleState(SlackState slackState)
        {
            HandleState(slackState, skipMyself: true);            
        }

        private void HandleState(SlackState slackState, bool skipMyself)
        {
            var continueToNextProcessor = true;
            if (!skipMyself)
            {
                continueToNextProcessor = this.HandleStateRaw(slackState);
            }

            if (continueToNextProcessor && Next != null)
            {
                Next.HandleState(slackState);
            }
        }

    }
}