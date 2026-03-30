using System;
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
        /// Retrieves the meal plan specifically for the current day for a given user.
        /// </summary>
        public async Task<MealPlan> GetTodaysMealPlanAsync(int userId)
        {
            try
            {
                // Fetch all plans for this user from the newly merged repository
                var allPlans = await _mealPlanRepository.GetAll();

                // Filter specifically for today's date
                var today = DateTime.Today;

                return allPlans.FirstOrDefault(p =>
                    p.UserId == userId &&
                    p.CreatedAt.Date == today.Date);
            }
            catch (Exception ex)
            {
                // Log the exception 
                Console.WriteLine($"Error fetching today's meal plan: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Generates a new meal plan and saves it to the database.
        /// </summary>
        public async Task<bool> GenerateNewMealPlanAsync(int userId)
        {
            try
            {
                // Create a new meal plan entry
                var newPlan = new MealPlan
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    GoalType = "Maintenance" // Default goal, can be updated later from UserData
                };

                // Save it using the repository
                await _mealPlanRepository.Add(newPlan);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating meal plan: {ex.Message}");
                return false;
            }
        }
    }
}