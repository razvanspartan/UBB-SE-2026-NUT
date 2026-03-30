using System;
using System.Collections.Generic;
using System.Linq;
using TeamNut.Models;
using TeamNut.Repositories;
using TeamNut.Services;

public class CalorieLogService : ICalorieLogService
{
    private readonly CalorieLogRepository _repository;

    public CalorieLogService()
    {
        _repository = new CalorieLogRepository();
    }

    // ✅ Get daily log
    public CalorieLog GetDailyLog(int userId, DateTime date)
    {
        return _repository.GetDailyLog(userId, date);
    }

    // ✅ Save or update daily log
    public void SaveDailyLog(CalorieLog log)
    {
        var existing = _repository.GetDailyLog(log.UserId, log.Date);

        if (existing != null)
        {
            _repository.Update(log);
        }
        else
        {
            _repository.Insert(log);
        }
    }

    // ✅ Get weekly totals
    public CalorieLog GetWeeklyTotals(int userId, DateTime date)
    {
        DateTime startOfWeek = GetStartOfWeek(date);
        DateTime endOfWeek = startOfWeek.AddDays(6);

        var logs = _repository.GetLogsInRange(userId, startOfWeek, endOfWeek);

        if (logs == null || logs.Count == 0)
            return new CalorieLog();

        return new CalorieLog
        {
            CaloriesConsumed = logs.Sum(x => x.CaloriesConsumed),
            CaloriesBurnt = logs.Sum(x => x.CaloriesBurnt),
            Protein = logs.Sum(x => x.Protein),
            Carbs = logs.Sum(x => x.Carbs),
            Fats = logs.Sum(x => x.Fats)
        };
    }

    // ✅ Check if a day has passed since meal plan
    public bool HasDayPassed(DateTime mealPlanDate)
    {
        return (DateTime.Now.Date - mealPlanDate.Date).TotalDays >= 1;
    }

    // ✅ Helper: Start of week (Monday)
    private DateTime GetStartOfWeek(DateTime date)
    {
        int diff = date.DayOfWeek - DayOfWeek.Monday;

        if (diff < 0)
            diff += 7;

        return date.AddDays(-diff).Date;
    }
}