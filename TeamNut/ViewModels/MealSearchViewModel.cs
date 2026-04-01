using System.Collections.Generic;
using TeamNut.Models;

namespace TeamNut.ViewModels
{
    public class MealSearchViewModel
    {
        private List<Meal> meals;

        public MealSearchViewModel()
        {
            meals = new List<Meal>();
        }

        public List<Meal> SearchMeals(MealFilter filter)
        {
            return meals;
        }

        public void ToggleFavorite(Meal meal)
        {
            if (meal != null)
                meal.IsFavorite = !meal.IsFavorite;
        }
    }
}