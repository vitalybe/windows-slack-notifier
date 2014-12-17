namespace SlackWindowsTray
{
    class StateProcessorsChain
    {
        private StateProcessorBase _root = new StartProcessor();

        private void AddProcessor(StateProcessorBase currentProcessor, StateProcessorBase newProcessor)
        {
            if (currentProcessor.Next == null || currentProcessor.Next.Priority >= newProcessor.Priority)
            {
                newProcessor.Next = currentProcessor.Next;
                currentProcessor.Next = newProcessor;

                newProcessor.OnAdd();
            }
            else
            {
                AddProcessor(currentProcessor.Next, newProcessor);
            }
        }

        private void RemoveProcessor(StateProcessorBase currentProcessor, StateProcessorBase removedProcessor)
        {
            if (currentProcessor.Next == removedProcessor)
            {
                currentProcessor.Next.OnRemove();
                currentProcessor.Next = removedProcessor.Next;
            }
            else if (currentProcessor.Next != null)
            {
                RemoveProcessor(currentProcessor.Next, removedProcessor);
            }
        }

        public void AddProcessor(StateProcessorBase newProcessor)
        {
            if (_root == null)
            {
                _root = newProcessor;
            }
            else
            {
                AddProcessor(_root, newProcessor);
            }
        }

        public void RemoveProcessor(StateProcessorBase removedProcessor)
        {
            if (_root == removedProcessor)
            {
                _root = null;
            }
            else
            {
                RemoveProcessor(_root, removedProcessor);
            }
        }

        public void HandleState(SlackNotifierStates state)
        {
            if (_root != null)
            {
                _root.HandleState(state);
            }
        }
    }

    public enum StateProcessorPriorityEnum
    {
        Start,
        Snoozing,
        Animation,
        Callback,
    }

    abstract class StateProcessorBase
    {
        protected abstract bool HandleStateRaw(SlackNotifierStates state);

        public virtual void OnAdd() { }
        public virtual void OnRemove() { }

        public abstract StateProcessorPriorityEnum Priority { get; }

        public StateProcessorBase Next { get; set; }
        
        public void HandleState(SlackNotifierStates state)
        {
            HandleState(state, skipMyself: false);
        }

        protected void NextHandleState(SlackNotifierStates state)
        {
            HandleState(state, skipMyself: true);            
        }

        private void HandleState(SlackNotifierStates state, bool skipMyself)
        {
            var continueToNextProcessor = true;
            if (!skipMyself)
            {
                continueToNextProcessor = this.HandleStateRaw(state);
            }

            if (continueToNextProcessor && Next != null)
            {
                Next.HandleState(state);
            }
        }

    }
}