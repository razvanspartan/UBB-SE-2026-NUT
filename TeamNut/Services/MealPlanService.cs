using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamNut.Models;
using TeamNut.Services;
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

        public async Task GenerateDailyPlan(int userId, string goal, List<int> selectedMealIds)
        {
            
            var newPlan = new MealPlan
            {
                UserId = userId,
                CreatedAt = DateTime.Now,
                GoalType = goal
            };

            await _mealPlanRepository.Add(newPlan);
        }

        public async Task<IEnumerable<MealPlan>> GetHistory(int userId)
        {
            
            return await _mealPlanRepository.GetAll();
        }
    }
}