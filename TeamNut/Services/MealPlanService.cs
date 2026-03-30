using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            // Initializing repository following the team's pattern
            _mealPlanRepository = new MealPlanRepository();
        }

        public async Task<List<MealPlan>> GetMealPlansByUserIdAsync(int userId)
        {
            var allPlans = await _mealPlanRepository.GetAll();

            // Filter plans for the specific user and ensure it's not null
            if (allPlans == null) return new List<MealPlan>();

            return allPlans.Where(p => p.UserId == userId).ToList();
        }

        public async Task<bool> AddMealPlanAsync(MealPlan plan)
        {
            if (plan == null) return false;

            await _mealPlanRepository.Add(plan);
            return true;
        }

        // Method to simulate the core generation logic needed for the frontend
        public async Task<bool> GenerateNewMealPlanAsync(int userId)
        {
            // For now, we simulate a successful generation call to the backend
            await Task.Delay(2000);
            return true;
        }
    }
}