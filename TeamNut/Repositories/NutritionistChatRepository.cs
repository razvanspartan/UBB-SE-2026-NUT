using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamNut.Models;

namespace TeamNut.Repositories
{
    public class NutritionistChatRepository
    {
        private readonly string _connectionString = DbConfig.ConnectionString;

        public async Task<int> CreateConversationAsync(NutritionistChatConversation conv)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("INSERT INTO ChatConversations (user_id, nutritionist_id, created_at) OUTPUT INSERTED.id VALUES (@u, @n, @c)", conn);
            cmd.Parameters.AddWithValue("@u", conv.UserId);
            cmd.Parameters.AddWithValue("@n", conv.NutritionistId.HasValue ? (object)conv.NutritionistId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@c", conv.CreatedAt);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<IEnumerable<NutritionistChatConversation>> GetConversationsForUserAsync(int userId)
        {
            var list = new List<NutritionistChatConversation>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT id, user_id, nutritionist_id, created_at FROM ChatConversations WHERE user_id=@u", conn);
            cmd.Parameters.AddWithValue("@u", userId);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new NutritionistChatConversation
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    NutritionistId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                    CreatedAt = reader.GetDateTimeOffset(3)
                });
            }
            return list;
        }

        public async Task<IEnumerable<NutritionistChatConversation>> GetAllConversationsAsync()
        {
            var list = new List<NutritionistChatConversation>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT id, user_id, nutritionist_id, created_at FROM ChatConversations", conn);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new NutritionistChatConversation
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    NutritionistId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                    CreatedAt = reader.GetDateTimeOffset(3)
                });
            }
            return list;
        }

        public async Task<IEnumerable<NutritionistChatMessage>> GetMessagesAsync(int conversationId)
        {
            var list = new List<NutritionistChatMessage>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT id, conversation_id, sender_id, sender_role, text, timestamp, is_read FROM ChatMessages WHERE conversation_id=@c ORDER BY timestamp", conn);
            cmd.Parameters.AddWithValue("@c", conversationId);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new NutritionistChatMessage
                {
                    Id = reader.GetInt32(0),
                    ConversationId = reader.GetInt32(1),
                    SenderId = reader.GetInt32(2),
                    SenderRole = reader.GetString(3),
                    Text = reader.GetString(4),
                    Timestamp = reader.GetDateTimeOffset(5),
                    IsRead = reader.GetBoolean(6)
                });
            }
            return list;
        }

        public async Task<int> AddMessageAsync(NutritionistChatMessage msg)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("INSERT INTO ChatMessages (conversation_id, sender_id, sender_role, text, timestamp, is_read) OUTPUT INSERTED.id VALUES (@c, @s, @r, @t, @ts, @read)", conn);
            cmd.Parameters.AddWithValue("@c", msg.ConversationId);
            cmd.Parameters.AddWithValue("@s", msg.SenderId);
            cmd.Parameters.AddWithValue("@r", msg.SenderRole ?? string.Empty);
            cmd.Parameters.AddWithValue("@t", msg.Text ?? string.Empty);
            cmd.Parameters.AddWithValue("@ts", msg.Timestamp);
            cmd.Parameters.AddWithValue("@read", msg.IsRead);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task MarkMessagesReadAsync(int conversationId, int readerId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("UPDATE ChatMessages SET is_read = 1 WHERE conversation_id = @c AND sender_id <> @r", conn);
            cmd.Parameters.AddWithValue("@c", conversationId);
            cmd.Parameters.AddWithValue("@r", readerId);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
