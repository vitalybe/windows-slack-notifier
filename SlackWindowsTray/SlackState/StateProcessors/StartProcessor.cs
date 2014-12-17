namespace SlackWindowsTray
{
    class StartProcessor : StateProcessorBase
    {
        protected override StateProcessorPriorityEnum Priority
        {
            get { return StateProcessorPriorityEnum.Start; }
        }

        protected override bool HandleStateRaw(SlackNotifierStates state)
        {
            return true;
        }
    }
}