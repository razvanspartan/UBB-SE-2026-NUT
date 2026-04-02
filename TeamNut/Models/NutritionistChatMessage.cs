using System;

namespace TeamNut.Models
{
    public class NutritionistChatMessage
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string SenderRole { get; set; }
        public string Text { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public bool IsRead { get; set; }
    }
}
