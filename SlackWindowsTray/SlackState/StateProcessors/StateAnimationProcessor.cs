using System;
using System.Timers;

namespace SlackWindowsTray
{
    class StateAnimationProcessor : StateProcessorBase
    {
        private SlackNotifierStates _lastState;
        private readonly Timer _animationTimer = new Timer();
        private bool _animationIconBlink = true;

        public StateAnimationProcessor()
        {
            _animationTimer.Interval = 500;
            _animationTimer.Elapsed += AnimationTimerOnTick;
            _animationTimer.Enabled = false;
        }

        private void AnimationTimerOnTick(object sender, EventArgs eventArgs)
        {
            var blinkState = _animationIconBlink ? SlackNotifierStates.AllRead : _lastState;
            NextHandleState(blinkState);
            _animationIconBlink = !_animationIconBlink;
        }


        protected override bool HandleStateRaw(SlackNotifierStates state)
        {
            _lastState = state;

            // Start the animation if possible and enabled
            var canAnimateIcon = state == SlackNotifierStates.ImportantUnread ||
                                 state == SlackNotifierStates.Unread;
            _animationTimer.Enabled = canAnimateIcon;

            return true;
        }

        public override StateProcessorPriorityEnum Priority { get { return StateProcessorPriorityEnum.Animation; } }
    }
}