using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNut.Services;

namespace TeamNut.ViewModels
{
    public class LoggingViewModel : INotifyPropertyChanged
    {
        private readonly ICalorieLogService _service;

        public string DailyStatusText { get; set; }

        public double DailyCalories { get; set; }
        public double DailyBurnt { get; set; }
        public double DailyProtein { get; set; }
        public double DailyCarbs { get; set; }
        public double DailyFats { get; set; }

        public double WeeklyCalories { get; set; }
        public double WeeklyBurnt { get; set; }
        public double WeeklyProtein { get; set; }
        public double WeeklyCarbs { get; set; }
        public double WeeklyFats { get; set; }

        public LoggingViewModel(ICalorieLogService service)
        {
            _service = service;
        }

        public async Task Load(int userId, DateTime mealPlanDate)
        {
            if (!_service.HasDayPassed(mealPlanDate))
            {
                DailyStatusText = "Not enough information to display";
                OnPropertyChanged(nameof(DailyStatusText));
                return;
            }

            var today = DateTime.Now.Date;

            var daily = await _service.GetDailyLog(userId, today);
            var weekly = await _service.GetWeeklyTotals(userId, StartOfWeek(today));

            if (daily != null)
            {
                DailyCalories = daily.CaloriesConsumed;
                DailyBurnt = daily.CaloriesBurnt;
                DailyProtein = daily.Protein;
                DailyCarbs = daily.Carbs;
                DailyFats = daily.Fats;
            }

            WeeklyCalories = weekly.CaloriesConsumed;
            WeeklyBurnt = weekly.CaloriesBurnt;
            WeeklyProtein = weekly.Protein;
            WeeklyCarbs = weekly.Carbs;
            WeeklyFats = weekly.Fats;

            NotifyAll();
        }

        private DateTime StartOfWeek(DateTime date)
        {
            int diff = date.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0) diff += 7;
            return date.AddDays(-diff).Date;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void NotifyAll()
        {
            OnPropertyChanged(nameof(DailyCalories));
            OnPropertyChanged(nameof(DailyBurnt));
            OnPropertyChanged(nameof(DailyProtein));
            OnPropertyChanged(nameof(DailyCarbs));
            OnPropertyChanged(nameof(DailyFats));

            OnPropertyChanged(nameof(WeeklyCalories));
            OnPropertyChanged(nameof(WeeklyBurnt));
            OnPropertyChanged(nameof(WeeklyProtein));
            OnPropertyChanged(nameof(WeeklyCarbs));
            OnPropertyChanged(nameof(WeeklyFats));
        }
    }
}
