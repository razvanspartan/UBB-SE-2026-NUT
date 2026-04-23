using System;

namespace TeamNut.Models
{
    public class Conversation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool HasUnanswered { get; set; }
    }
}
