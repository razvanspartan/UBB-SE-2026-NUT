using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using TeamNut.Models;

namespace TeamNut.Repositories
{
    public class ShoppingListRepository : IRepository<ShoppingItem>
    {
        private readonly string _connectionString = DbConfig.ConnectionString;

        public async Task Add(ShoppingItem item)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"INSERT INTO ShoppingItems (user_id, ingredient_id, quantity_grams, is_checked)
                             VALUES (@userId, @ingredientId, @quantityGrams, @isChecked)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", item.UserId);
            cmd.Parameters.AddWithValue("@ingredientId", item.IngredientId);
            cmd.Parameters.AddWithValue("@quantityGrams", item.QuantityGrams);
            cmd.Parameters.AddWithValue("@isChecked", item.IsChecked ? 1 : 0);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<ShoppingItem>> GetAll()
        {
            var items = new List<ShoppingItem>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"SELECT s.id, s.user_id, s.ingredient_id, s.quantity_grams, s.is_checked, i.name AS ingredient_name 
                             FROM ShoppingItems s 
                             JOIN Ingredients i ON s.ingredient_id = i.food_id";

            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                items.Add(MapReaderToItem(reader));
            }

            return items;
        }

        public async Task<ShoppingItem> GetById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"SELECT s.id, s.user_id, s.ingredient_id, s.quantity_grams, s.is_checked, i.name AS ingredient_name 
                             FROM ShoppingItems s 
                             JOIN Ingredients i ON s.ingredient_id = i.food_id
                             WHERE s.id = @id";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapReaderToItem(reader);
            }

            return null;
        }

        public async Task<List<ShoppingItem>> GetAllByUserId(int userId)
        {
            var items = new List<ShoppingItem>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"SELECT s.id, s.user_id, s.ingredient_id, s.quantity_grams, s.is_checked, i.name AS ingredient_name 
                             FROM ShoppingItems s 
                             JOIN Ingredients i ON s.ingredient_id = i.food_id
                             WHERE s.user_id = @userId";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", userId);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                items.Add(MapReaderToItem(reader));
            }

            return items;
        }

        public async Task Update(ShoppingItem item)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"UPDATE ShoppingItems 
                             SET ingredient_id = @ingredientId, quantity_grams = @quantityGrams, is_checked = @isChecked
                             WHERE id = @id";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", item.Id);
            cmd.Parameters.AddWithValue("@ingredientId", item.IngredientId);
            cmd.Parameters.AddWithValue("@quantityGrams", item.QuantityGrams);
            cmd.Parameters.AddWithValue("@isChecked", item.IsChecked ? 1 : 0);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Delete(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = "DELETE FROM ShoppingItems WHERE id = @id";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> GenerateFromMealPlanAsync(int userId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string query = @"
                INSERT INTO ShoppingItems (user_id, ingredient_id, quantity_grams, is_checked)
                SELECT @userId, mi.food_id, SUM(mi.quantity), 0
                FROM MealPlan mp
                JOIN MealPlanMeal mpm ON mp.mealplan_id = mpm.mealPlanId
                JOIN MealsIngredients mi ON mpm.mealId = mi.meal_id
                WHERE mp.user_id = @userId 
                  AND mp.created_at >= CAST(GETDATE() AS DATE)
                  AND NOT EXISTS (
                      SELECT 1 FROM Inventory inv 
                      WHERE inv.user_id = @userId AND inv.ingredient_id = mi.food_id
                  )
                  AND NOT EXISTS (
                      SELECT 1 FROM ShoppingItems si 
                      WHERE si.user_id = @userId AND si.ingredient_id = mi.food_id
                  )
                GROUP BY mi.food_id";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", userId);
            
            int rowsInserted = await cmd.ExecuteNonQueryAsync();
            return rowsInserted;
        }

        private ShoppingItem MapReaderToItem(SqlDataReader reader)
        {
            return new ShoppingItem
            {
                Id = Convert.ToInt32(reader["id"]),
                UserId = Convert.ToInt32(reader["user_id"]),
                IngredientId = Convert.ToInt32(reader["ingredient_id"]),
                QuantityGrams = Convert.ToDouble(reader["quantity_grams"]),
                IngredientName = reader["ingredient_name"].ToString(),
                IsChecked = Convert.ToBoolean(reader["is_checked"])
            };
        }
    }
}
