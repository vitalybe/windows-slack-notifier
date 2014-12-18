using System.Collections.Generic;

namespace SlackWindowsTray
{
    public struct SlackState
    {
        public SlackState(List<ChatState> chatStates) : this()
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

        public SlackState(TrayStates trayState) : this()
        {
            ChatStates = new List<ChatState>();
            TrayState = trayState;
        }

        public List<ChatState> ChatStates { get; private set; }
        public TrayStates TrayState { get; set; }
    }
}