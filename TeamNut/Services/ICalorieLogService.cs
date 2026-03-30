using System;
using TeamNut.Models;

public interface ICalorieLogService
{
    // ✅ Get log for a specific day
    CalorieLog GetDailyLog(int userId, DateTime date);

    // ✅ Save or update a daily log
    void SaveDailyLog(CalorieLog log);

    // ✅ Get totals for the current week
    CalorieLog GetWeeklyTotals(int userId, DateTime date);

    // ✅ Check if at least one day passed since meal plan
    bool HasDayPassed(DateTime mealPlanDate);
}