using System.Collections.Generic;
using NutApp.Domain;
using NutApp.Backend.Repositories;

namespace NutApp.Backend.Services
{
    public class MealService
    {
        private readonly MealRepository _mealRepository;

        
        public MealService()
        {
            _mealRepository = new MealRepository();
        }

        
        public List<Meal> GetMeals(string filter = null)
        {
            
            if (string.IsNullOrWhiteSpace(filter) || filter == "All")
            {
                return _mealRepository.GetAllMeals();
            }

            
            return _mealRepository.GetMealsByFilter(filter);
        }

        
        public void ToggleFavorite(int userId, int mealId, bool currentlyFavorited)
        {
            if (currentlyFavorited)
            {
                _mealRepository.RemoveFavorite(userId, mealId);
            }
            else
            {
                _mealRepository.AddFavorite(userId, mealId);
            }
        }

        public List<Meal> GetUserFavorites(int userId)
        {
            return _mealRepository.GetUserFavorites(userId);
        }
    }
}