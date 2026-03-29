using System.Collections.Generic;
using System.Linq;
using TeamNut.Models;

namespace TeamNut.Services
{
    public class MealService
    {
        public List<Meal> GetMeals(MealFilter filter)
        {
            var meals = new List<Meal>
            {
                new Meal { Name = "Chicken Rice", Calories = 500 },
                new Meal { Name = "Beef Burger", Calories = 700 },
                new Meal { Name = "Salad", Calories = 200 },
                new Meal { Name = "Pasta", Calories = 600 }
            };

            if (filter != null && !string.IsNullOrEmpty(filter.Query))
            {
                meals = meals
                    .Where(m => m.Name.ToLower().Contains(filter.Query.ToLower()))
                    .ToList();
            }

            return meals;
        }
    }
}