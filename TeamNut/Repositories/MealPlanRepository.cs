using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamNut.Repositories; 
using TeamNut.Models;

namespace TeamNut.Repositories
{
    internal class MealPlanRepository : IRepository<MealPlan>
    {
        private readonly string _connectionString = DbConfig.ConnectionString;

        public async Task<MealPlan> GetById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            const string sql = "SELECT * FROM MealPlan WHERE mealplan_id = @id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync()) return MapReaderToMealPlan(reader);
            return null;
        }

        public async Task<IEnumerable<MealPlan>> GetAll()
        {
            var plans = new List<MealPlan>();
            using var conn = new SqlConnection(_connectionString);
            const string sql = "SELECT * FROM MealPlan";
            using var cmd = new SqlCommand(sql, conn);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) plans.Add(MapReaderToMealPlan(reader));
            return plans;
        }

        public async Task Add(MealPlan entity)
        {
            using var conn = new SqlConnection(_connectionString);
            const string sql = @"INSERT INTO MealPlan (user_id, created_at, [goal type]) 
                                VALUES (@uid, @created, @goal)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@uid", entity.UserId);
            cmd.Parameters.AddWithValue("@created", entity.CreatedAt);
            cmd.Parameters.AddWithValue("@goal", entity.GoalType ?? (object)DBNull.Value);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Update(MealPlan entity)
        {
            using var conn = new SqlConnection(_connectionString);
            const string sql = @"UPDATE MealPlan SET [goal type] = @goal 
                                 WHERE mealplan_id = @id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.Parameters.AddWithValue("@goal", entity.GoalType);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Delete(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            const string sql = "DELETE FROM MealPlan WHERE mealplan_id = @id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> GenerateDefaultDailyMealPlan(int userId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var transaction = conn.BeginTransaction();

            try
            {
                const string insertPlanSql = @"INSERT INTO MealPlan (user_id, created_at, goal_type) 
                                               OUTPUT INSERTED.mealplan_id
                                               VALUES (@uid, @created, @goal)";
                using var planCmd = new SqlCommand(insertPlanSql, conn, transaction);
                planCmd.Parameters.AddWithValue("@uid", userId);
                planCmd.Parameters.AddWithValue("@created", DateTime.Now);
                planCmd.Parameters.AddWithValue("@goal", "general");

                int mealPlanId = (int)await planCmd.ExecuteScalarAsync();

                const string getMealsSql = @"
                    SELECT TOP 1 meal_id FROM Meals ORDER BY NEWID();
                    SELECT TOP 1 meal_id FROM Meals ORDER BY NEWID();
                    SELECT TOP 1 meal_id FROM Meals ORDER BY NEWID();";

                var mealIds = new List<int>();
                var mealTypes = new[] { "breakfast", "lunch", "dinner" };

                using var mealsCmd = new SqlCommand(getMealsSql, conn, transaction);
                using var reader = await mealsCmd.ExecuteReaderAsync();

                int index = 0;
                do
                {
                    while (await reader.ReadAsync())
                    {
                        mealIds.Add(Convert.ToInt32(reader["meal_id"]));
                    }
                    index++;
                } while (await reader.NextResultAsync());

                reader.Close();

                const string insertMealPlanMealSql = @"INSERT INTO MealPlanMeal (mealPlanId, mealId, mealType, assigned_at, isConsumed) 
                                                       VALUES (@planId, @mealId, @mealType, @assignedAt, 0)";

                for (int i = 0; i < mealIds.Count && i < mealTypes.Length; i++)
                {
                    using var mealPlanMealCmd = new SqlCommand(insertMealPlanMealSql, conn, transaction);
                    mealPlanMealCmd.Parameters.AddWithValue("@planId", mealPlanId);
                    mealPlanMealCmd.Parameters.AddWithValue("@mealId", mealIds[i]);
                    mealPlanMealCmd.Parameters.AddWithValue("@mealType", mealTypes[i]);
                    mealPlanMealCmd.Parameters.AddWithValue("@assignedAt", DateTime.Now);
                    await mealPlanMealCmd.ExecuteNonQueryAsync();
                }

                transaction.Commit();
                return mealPlanId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<Meal>> GetMealsForMealPlan(int mealPlanId)
        {
            var meals = new List<Meal>();
            using var conn = new SqlConnection(_connectionString);

            const string sql = @"SELECT m.*, mpm.mealType, mpm.isConsumed 
                                FROM Meals m
                                INNER JOIN MealPlanMeal mpm ON m.meal_id = mpm.mealId
                                WHERE mpm.mealPlanId = @planId
                                ORDER BY 
                                    CASE mpm.mealType 
                                        WHEN 'breakfast' THEN 1 
                                        WHEN 'lunch' THEN 2 
                                        WHEN 'dinner' THEN 3 
                                        ELSE 4 
                                    END";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@planId", mealPlanId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                meals.Add(new Meal
                {
                    Id = Convert.ToInt32(reader["meal_id"]),
                    Name = reader["name"].ToString(),
                    ImageUrl = reader["imageUrl"]?.ToString(),
                    IsKeto = Convert.ToBoolean(reader["isKeto"]),
                    IsVegan = Convert.ToBoolean(reader["isVegan"]),
                    IsNutFree = Convert.ToBoolean(reader["isNutFree"]),
                    IsLactoseFree = Convert.ToBoolean(reader["isLactoseFree"]),
                    IsGlutenFree = Convert.ToBoolean(reader["isGlutenFree"]),
                    Description = reader["description"]?.ToString()
                });
            }

            return meals;
        }


        private MealPlan MapReaderToMealPlan(SqlDataReader reader)
        {
            return new MealPlan
            {
                Id = Convert.ToInt32(reader["mealplan_id"]),
                UserId = Convert.ToInt32(reader["user_id"]),
                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                GoalType = reader["goal type"]?.ToString()
            };
        }
    }
}