using System;
using System.Threading.Tasks;
using TeamNut.Models;

namespace TeamNut.Services
{
    public interface ICalorieLogService
    {
        Task<CalorieLog> GetDailyLog(int userId, DateTime date);

        Task SaveDailyLog(CalorieLog log);

        Task<CalorieLog> GetWeeklyTotals(int userId, DateTime date);

        bool HasDayPassed(DateTime mealPlanDate);
    }
}