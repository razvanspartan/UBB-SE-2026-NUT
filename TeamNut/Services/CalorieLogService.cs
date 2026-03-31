using System;
using System.Linq;
using System.Threading.Tasks;
using TeamNut.Models;
using TeamNut.Repositories;

namespace TeamNut.Services
{
    public class CalorieLogService : ICalorieLogService
    {
        private readonly CalorieLogRepository _repository;

        public CalorieLogService()
        {
            _repository = new CalorieLogRepository();
        }

        public async Task<CalorieLog> GetDailyLog(int userId, DateTime date)
        {
            return await _repository.GetByUserAndDate(userId, date);
        }

        public async Task SaveDailyLog(CalorieLog log)
        {
            var existing = await _repository.GetByUserAndDate(log.UserId, log.Date);

            if (existing != null)
            {
                log.Id = existing.Id;
                await _repository.Update(log);
            }
            else
            {
                await _repository.Add(log);
            }
        }

        public async Task<CalorieLog> GetWeeklyTotals(int userId, DateTime date)
        {
            DateTime startOfWeek = GetStartOfWeek(date);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            var logs = await _repository.GetByUserAndDateRange(userId, startOfWeek, endOfWeek);

            if (logs == null || !logs.Any())
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

        public bool HasDayPassed(DateTime mealPlanDate)
        {
            return (DateTime.Now.Date - mealPlanDate.Date).TotalDays >= 1;
        }

        private DateTime GetStartOfWeek(DateTime date)
        {
            int diff = date.DayOfWeek - DayOfWeek.Monday;

            if (diff < 0)
                diff += 7;

            return date.AddDays(-diff).Date;
        }
    }
}