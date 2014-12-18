using System;
using System.Collections.Generic;
using System.Timers;

namespace SlackWindowsTray
{
    class StateAnimationProcessor : StateProcessorBase
    {
        private TrayStates _lastTrayState;
        private SlackState _lastSlackState;

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
            _animationIconBlink = !_animationIconBlink;

            _lastSlackState.TrayState = _animationIconBlink ? TrayStates.AllRead : _lastTrayState;
            NextHandleState(_lastSlackState);
        }


        protected override bool HandleStateRaw(SlackState slackState)
        {
            _lastSlackState = slackState;
            _lastTrayState = slackState.TrayState;

            // Start the animation if possible and enabled
            var canAnimateIcon = _lastTrayState == TrayStates.ImportantUnread ||
                                 _lastTrayState == TrayStates.Unread;
            _animationTimer.Enabled = canAnimateIcon;

            return true;
        }

        public override StateProcessorPriorityEnum Priority { get { return StateProcessorPriorityEnum.Animation; } }
    }
}