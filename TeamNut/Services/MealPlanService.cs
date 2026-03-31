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

        // Generates a new personalized daily meal plan for the user
        public async Task<bool> GenerateNewMealPlanAsync(int userId)
        {
            try
            {
                // Calls the complex repository method to calculate and save the plan
                int mealPlanId = await _mealPlanRepository.GeneratePersonalizedDailyMealPlan(userId);

                // If it returns an ID greater than 0, the generation and DB insert were successful
                return mealPlanId > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating meal plan: {ex.Message}");
                return false;
            }
        }

        // Fetches ONLY the meal plan generated today for the specific user
        public async Task<MealPlan> GetTodaysMealPlanAsync(int userId)
        {
            try
            {
                // Get all plans (this is fast enough for now, but ideally a SQL WHERE clause would be better)
                var allPlans = await _mealPlanRepository.GetAll();

                // Filter in memory to find today's plan for this specific user
                var todaysPlan = allPlans.FirstOrDefault(p =>
                    p.UserId == userId &&
                    p.CreatedAt.Date == DateTime.Today);

                return todaysPlan;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching today's meal plan: {ex.Message}");
                return null;
            }
        }

        // Fetches the actual meals (breakfast, lunch, dinner) attached to a specific Meal Plan ID
        public async Task<List<Meal>> GetMealsForPlanAsync(int mealPlanId)
        {
            try
            {
                return await _mealPlanRepository.GetMealsForMealPlan(mealPlanId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching meals for plan: {ex.Message}");
                return new List<Meal>();
            }
        }
    }
}