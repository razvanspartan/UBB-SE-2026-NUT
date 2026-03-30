using System;
using System.Collections.Generic;
using System.Data.SQLite;
using TeamNut.Models;
using TeamNut.Repositories;

public class CalorieLogRepository
{
    private readonly DbConfig _db;

    public CalorieLogRepository()
    {
        _db = new DbConfig();
    }

    // ✅ Get log for a specific day
    public CalorieLog GetDailyLog(int userId, DateTime date)
    {
        using var conn = _db.GetConnection();
        conn.Open();

        string query = @"SELECT * FROM CalorieLogs 
                         WHERE UserId = @userId AND Date = @date";

        using var cmd = new SQLiteCommand(query, conn);
        cmd.Parameters.AddWithValue("@userId", userId);
        cmd.Parameters.AddWithValue("@date", date.Date);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapReaderToLog(reader);
        }

        return null;
    }

    // ✅ Insert new log
    public void Insert(CalorieLog log)
    {
        using var conn = _db.GetConnection();
        conn.Open();

        string query = @"INSERT INTO CalorieLogs 
                        (UserId, Date, CaloriesConsumed, CaloriesBurnt, Protein, Carbs, Fats)
                        VALUES (@userId, @date, @caloriesConsumed, @caloriesBurnt, @protein, @carbs, @fats)";

        using var cmd = new SQLiteCommand(query, conn);
        AddParameters(cmd, log);

        cmd.ExecuteNonQuery();
    }

    // ✅ Update existing log
    public void Update(CalorieLog log)
    {
        using var conn = _db.GetConnection();
        conn.Open();

        string query = @"UPDATE CalorieLogs 
                         SET CaloriesConsumed = @caloriesConsumed,
                             CaloriesBurnt = @caloriesBurnt,
                             Protein = @protein,
                             Carbs = @carbs,
                             Fats = @fats
                         WHERE UserId = @userId AND Date = @date";

        using var cmd = new SQLiteCommand(query, conn);
        AddParameters(cmd, log);

        cmd.ExecuteNonQuery();
    }

    // ✅ Get logs in date range (for weekly totals)
    public List<CalorieLog> GetLogsInRange(int userId, DateTime start, DateTime end)
    {
        var logs = new List<CalorieLog>();

        using var conn = _db.GetConnection();
        conn.Open();

        string query = @"SELECT * FROM CalorieLogs 
                         WHERE UserId = @userId 
                         AND Date >= @start 
                         AND Date <= @end";

        using var cmd = new SQLiteCommand(query, conn);
        cmd.Parameters.AddWithValue("@userId", userId);
        cmd.Parameters.AddWithValue("@start", start.Date);
        cmd.Parameters.AddWithValue("@end", end.Date);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            logs.Add(MapReaderToLog(reader));
        }

        return logs;
    }

    // 🔧 Helper: Map DB row → object
    private CalorieLog MapReaderToLog(SQLiteDataReader reader)
    {
        return new CalorieLog
        {
            UserId = Convert.ToInt32(reader["UserId"]),
            Date = Convert.ToDateTime(reader["Date"]),
            CaloriesConsumed = Convert.ToDouble(reader["CaloriesConsumed"]),
            CaloriesBurnt = Convert.ToDouble(reader["CaloriesBurnt"]),
            Protein = Convert.ToDouble(reader["Protein"]),
            Carbs = Convert.ToDouble(reader["Carbs"]),
            Fats = Convert.ToDouble(reader["Fats"])
        };
    }

    // 🔧 Helper: Add parameters to command
    private void AddParameters(SQLiteCommand cmd, CalorieLog log)
    {
        cmd.Parameters.AddWithValue("@userId", log.UserId);
        cmd.Parameters.AddWithValue("@date", log.Date.Date);
        cmd.Parameters.AddWithValue("@caloriesConsumed", log.CaloriesConsumed);
        cmd.Parameters.AddWithValue("@caloriesBurnt", log.CaloriesBurnt);
        cmd.Parameters.AddWithValue("@protein", log.Protein);
        cmd.Parameters.AddWithValue("@carbs", log.Carbs);
        cmd.Parameters.AddWithValue("@fats", log.Fats);
    }
}