using System.Collections.Generic;
using System.Linq;

namespace SlackWindowsTray
{
    class StateProcessorsChain
    {
        private List<StateProcessorBase> _processors = new List<StateProcessorBase>();
        private StateProcessorBase _root = null;


        public void AddProcessor(StateProcessorBase newProcessor)
        {
            var last = _processors.LastOrDefault();
            if (last != null)
            {
                last.Next = newProcessor;
            }

            _processors.Add(newProcessor);
        }

        public void HandleState(SlackState state)
        {
            if (_processors.Count > 0)
            {
                _processors.First().HandleState(state);
            }
        }
    }
}