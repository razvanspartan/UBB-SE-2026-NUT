using System;

namespace TeamNut.Models
{
    public class NutritionistChatConversation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? NutritionistId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
