using System.Collections.Generic;
using TeamNut.Models;
using TeamNut.Services;

namespace TeamNut.ViewModels
{
    public class MealSearchViewModel
    {
        private readonly MealService _service;

        public MealSearchViewModel()
        {
            _service = new MealService();
        }

        public List<Meal> SearchMeals(MealFilter filter)
        {
            return _service.GetMeals(filter);
        }

        public void ToggleFavorite(Meal meal)
        {
            meal.IsFavorite = !meal.IsFavorite;
        }
    }
}