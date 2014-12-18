namespace SlackWindowsTray
{
    public enum TrayStates
    {
        DisconnectedFromExtension = -2,
        DisconnectedFromSlack = -1,
        AllRead = 0,
        Unread = 1,
        ImportantUnread = 2
    }
}