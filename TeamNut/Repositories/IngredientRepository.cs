using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace TeamNut.Repositories
{
    public class IngredientRepository
    {
        private readonly string _connectionString = DbConfig.ConnectionString;

        public async Task<int> GetOrCreateIngredientIdAsync(string name)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string selectQuery = "SELECT food_id FROM Ingredients WHERE name = @name";
            using var selectCmd = new SqlCommand(selectQuery, conn);
            selectCmd.Parameters.AddWithValue("@name", name);

            var result = await selectCmd.ExecuteScalarAsync();
            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }

            string insertQuery = @"INSERT INTO Ingredients (name) VALUES (@name);
                                   SELECT SCOPE_IDENTITY();";
            using var insertCmd = new SqlCommand(insertQuery, conn);
            insertCmd.Parameters.AddWithValue("@name", name);
            
            var newId = await insertCmd.ExecuteScalarAsync();
            return Convert.ToInt32(newId);
        }
    }
}
