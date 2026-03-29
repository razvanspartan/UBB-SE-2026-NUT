using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamNut;
using TeamNut.Models;
namespace TeamNut.Repositories;



public class UserRepository : IRepository<User>
{
    private readonly string _connectionString = DbConfig.ConnectionString;
    public async Task<User> GetById(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("SELECT Id, Username, Password, Role FROM Users WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                Role = reader.GetString(3)
            };
        }
        return null;
    }
    public async Task AddUserData(UserData data)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(@"
            INSERT INTO UserData (user_id, weight, height, age, gender, goal, bmi, calorie_needs, protein_needs, carb_needs, fat_needs)
            VALUES (@userId, @w, @h, @a, @gen, @goal, @bmi, @cal, @pro, @carb, @fat)", conn);

        cmd.Parameters.AddWithValue("@userId", data.UserId);
        cmd.Parameters.AddWithValue("@w", data.Weight);
        cmd.Parameters.AddWithValue("@h", data.Height);
        cmd.Parameters.AddWithValue("@a", data.Age);
        cmd.Parameters.AddWithValue("@gen", data.Gender);
        cmd.Parameters.AddWithValue("@goal", data.Goal);
        cmd.Parameters.AddWithValue("@bmi", data.Bmi);
        cmd.Parameters.AddWithValue("@cal", data.CalorieNeeds);
        cmd.Parameters.AddWithValue("@pro", data.ProteinNeeds);
        cmd.Parameters.AddWithValue("@carb", data.CarbNeeds);
        cmd.Parameters.AddWithValue("@fat", data.FatNeeds);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task Add(User entity)
    {
        using var conn = new SqlConnection(_connectionString);
        string sql = "INSERT INTO Users (Username, Password, Role) OUTPUT INSERTED.Id VALUES (@u, @p, @r)";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@u", entity.Username);
        cmd.Parameters.AddWithValue("@p", entity.Password);
        cmd.Parameters.AddWithValue("@r", entity.Role);

        await conn.OpenAsync();

        var result = await cmd.ExecuteScalarAsync();

        if (result != null && result != DBNull.Value)
        {
            entity.Id = Convert.ToInt32(result);
        }
        else
        {
            throw new Exception("CRITICAL: Database did not return a new User ID.");
        }
    }

    public async Task Update(User entity)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("UPDATE Users SET Username=@u, Password=@p, Role=@r WHERE Id=@id", conn);

        cmd.Parameters.AddWithValue("@u", entity.Username);
        cmd.Parameters.AddWithValue("@p", entity.Password);
        cmd.Parameters.AddWithValue("@r", entity.Role);
        cmd.Parameters.AddWithValue("@id", entity.Id);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task Delete(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("DELETE FROM Users WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
    public async Task<User> GetByUsernameAndPassword(string username, string password)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("SELECT Id, Username, Password, Role FROM Users WHERE Username = @u AND Password = @p", conn);
        cmd.Parameters.AddWithValue("@u", username);
        cmd.Parameters.AddWithValue("@p", password);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                Role = reader.GetString(3)
            };
        }
        return null;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("Connection string is not initialized.");
        }

        var users = new List<User>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using var command = new SqlCommand("SELECT Id, Username, Password, Role FROM Users", connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    Role = reader.GetString(3)
                });
            }
        }
        return users;
    }
}
