namespace SlackWindowsTray
{
    public enum SlackNotifierStates
    {
        DisconnectedFromExtension = -2,
        DisconnectedFromSlack = -1,
        AllRead = 0,
        Unread = 1,
        ImportantUnread = 2
    }
}