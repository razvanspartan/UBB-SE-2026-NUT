using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamNut.Models; 
using TeamNut.Repositories;

namespace TeamNut.Services
{
    public class MealPlanService
    {
        private readonly MealPlanRepository _mealPlanRepository;

        public MealPlanService()
        {
            _mealPlanRepository = new MealPlanRepository();
        }

        /// <summary>
        /// Returns only the meal plan for the current day.
        /// </summary>
        public async Task<MealPlan> GetTodaysMealPlanAsync(int userId)
        {
            // Get all plans for this user from the repository lead just merged
            var allPlans = await _mealPlanRepository.GetAll();

            //Filter specifically for today's date
            var today = DateTime.Today;

            return allPlans.FirstOrDefault(p =>
                p.UserId == userId &&
                p.CreatedAt.Date == today.Date);
        }

        public async Task<bool> GenerateNewMealPlanAsync(int userId)
        {
            // Simulation of generation logic for the UI
            await Task.Delay(2000);
            return true;
        }
    }
}