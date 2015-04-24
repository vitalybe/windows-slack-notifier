namespace SlackWindowsTray
{
    class RtmMessageModel
    {
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string User { get; set; }
        public string MessageText { get; set; }
        public bool IsIncoming { get; set; }
    }
}