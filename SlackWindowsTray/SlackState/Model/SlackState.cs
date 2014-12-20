using System.Collections.Generic;
using System.Linq;

namespace SlackWindowsTray
{
    public class SlackState
    {
        private readonly List<ChatState> _chatStates;

        private void CalculateTrayState()
        {
            if (_chatStates.Exists(chatState => chatState.mention))
            {
                TrayState = TrayStates.ImportantUnread;
            }
            else if (_chatStates.Exists(chatState => chatState.unread))
            {
                TrayState = TrayStates.Unread;
            }
            else
            {
                TrayState = TrayStates.AllRead;
            }
        }

        public SlackState(List<ChatState> chatStates)
        {
            _chatStates = chatStates;

            CalculateTrayState();
        }

        public SlackState(TrayStates trayState)
        {
            _chatStates = new List<ChatState>();
            TrayState = trayState;
        }

        // Replaces an existing chat state if exists
        // Returns the original, if exists
        public IEnumerable<ChatState> ChatStates
        {
            get { return _chatStates; }
        }

        public TrayStates TrayState { get; set; }

        public ChatState ReplaceChatState(ChatState replacementChatState)
        {
            var originalChatState = ChatStates.FirstOrDefault(chatState => chatState.name == replacementChatState.name);
            if (originalChatState != null)
            {
                _chatStates.Remove(originalChatState);
                _chatStates.Add(replacementChatState);
            }

            CalculateTrayState();

            return originalChatState;
        }

        public SlackState Clone()
        {
            return (SlackState)this.MemberwiseClone();
        }
    }
}