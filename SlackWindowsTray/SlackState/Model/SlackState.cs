using System.Collections.Generic;

namespace SlackWindowsTray
{
    public class SlackState
    {
        public SlackState(List<ChatState> chatStates)
        {
            ChatStates = chatStates;

            if (chatStates.Exists(chatState => chatState.mention))
            {
                TrayState = TrayStates.ImportantUnread;
            }
            else if (chatStates.Exists(chatState => chatState.unread))
            {
                TrayState = TrayStates.Unread;
            }
            else
            {
                TrayState = TrayStates.AllRead;
            }
        }

        public SlackState(TrayStates trayState)
        {
            ChatStates = new List<ChatState>();
            TrayState = trayState;
        }

        public SlackState Clone()
        {
            return (SlackState)this.MemberwiseClone();
        }

        public List<ChatState> ChatStates { get; private set; }
        public TrayStates TrayState { get; set; }
    }
}