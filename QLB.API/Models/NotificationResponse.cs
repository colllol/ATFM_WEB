namespace QLB.API.Models
{
    public sealed class NotificationResponse : ReponseReportEntity
    {
        public int AIUnreadCount { get; set; }

        // Keep both names for existing bell and notification-list clients.
        public int AI_UNREAD_COUNT { get { return AIUnreadCount; } }
    }
}
