using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeamNut.Models;
using TeamNut.Repositories;

namespace TeamNut.Services
{
    public class NutritionistChatService
    {
        private readonly NutritionistChatRepository _repo;
        public NutritionistChatService()
        {
            _repo = new NutritionistChatRepository();
        }

        public async Task<IEnumerable<NutritionistChatConversation>> GetConversationsForCurrentUserAsync()
        {
            if (UserSession.UserId == null) return Enumerable.Empty<NutritionistChatConversation>();
            return await _repo.GetConversationsForUserAsync(UserSession.UserId.Value);
        }

        public async Task<IEnumerable<NutritionistChatConversation>> GetAllConversationsAsync()
        {
            return await _repo.GetAllConversationsAsync();
        }

        public async Task<IEnumerable<NutritionistChatMessage>> GetMessagesAsync(int conversationId)
        {
            return await _repo.GetMessagesAsync(conversationId);
        }

        public async Task<int> SendMessageAsync(int conversationId, string text)
        {
            if (UserSession.UserId == null) throw new InvalidOperationException("No user logged in.");

            // validation: non-empty, alphanumeric + punctuation, up to 1000 chars
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Message cannot be empty.");
            if (text.Length > 1000) throw new ArgumentException("Message exceeds maximum length of 1000.");
            // simple alphanumeric validation (allow common punctuation and spaces)
            var dq = '"'.ToString();
            var pattern = "^[a-zA-Z0-9 .,;:!?'" + dq + "()\\-\\n\\r]+$";
            if (!Regex.IsMatch(text, pattern))
            {
                throw new ArgumentException("Message contains invalid characters.");
            }

            var msg = new NutritionistChatMessage
            {
                ConversationId = conversationId,
                SenderId = UserSession.UserId.Value,
                SenderRole = UserSession.Role ?? "User",
                Text = text,
                Timestamp = DateTimeOffset.Now,
                IsRead = false
            };

            return await _repo.AddMessageAsync(msg);
        }

        public async Task<int> CreateConversationIfNotExistsAsync(int userId, int? nutritionistId = null)
        {
            var convs = await _repo.GetConversationsForUserAsync(userId);
            var conv = convs.FirstOrDefault();
            if (conv != null) return conv.Id;

            var newConv = new NutritionistChatConversation
            {
                UserId = userId,
                NutritionistId = nutritionistId,
                CreatedAt = DateTimeOffset.Now
            };
            return await _repo.CreateConversationAsync(newConv);
        }
    }
}
