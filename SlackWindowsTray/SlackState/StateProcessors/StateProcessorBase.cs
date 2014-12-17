namespace SlackWindowsTray
{
    abstract class StateProcessorBase
    {
        protected enum StateProcessorPriorityEnum
        {
            Start,
            Snoozing,
            Animation,
            Callback,
        }

        protected StateProcessorBase _nextProcessor = null;

        protected abstract StateProcessorPriorityEnum Priority { get; }

        protected abstract bool HandleStateRaw(SlackNotifierStates state);

        protected virtual void OnAdd() { }
        protected virtual void OnRemove() { }

        public void AddProcessor(StateProcessorBase newProcessor)
        {
            if (_nextProcessor == null || _nextProcessor.Priority >= newProcessor.Priority)
            {
                newProcessor._nextProcessor = _nextProcessor;
                this._nextProcessor = newProcessor;

                newProcessor.OnAdd();
            }
            else
            {
                _nextProcessor.AddProcessor(newProcessor);
            }
        }

        public void RemoveProcessor(StateProcessorBase removedProcessor)
        {
            if (_nextProcessor == removedProcessor)
            {
                _nextProcessor.OnRemove();
                this._nextProcessor = removedProcessor._nextProcessor;
            }
            else if(_nextProcessor != null)
            {
                _nextProcessor.RemoveProcessor(removedProcessor);
            }

        }

        
        public void HandleState(SlackNotifierStates state)
        {
            HandleState(state, skipMyself: false);
        }

        protected void NextHandleState(SlackNotifierStates state)
        {
            HandleState(state, skipMyself: true);            
        }

        private  void HandleState(SlackNotifierStates state, bool skipMyself)
        {
            var continueToNextProcessor = true;
            if (!skipMyself)
            {
                continueToNextProcessor = this.HandleStateRaw(state);
            }

            if (continueToNextProcessor && _nextProcessor != null)
            {
                _nextProcessor.HandleState(state);
            }
        }

    }
}