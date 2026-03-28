using System;
using System.Collections.Generic;
using System.Linq;
using TeamNut.Models;

namespace TeamNut.ViewModels
{
    public class MealSearchViewModel
    {
        private List<Meal> meals;
        private List<Meal> filteredMeals;

        private int pageSize = 5;
        private int currentPage = 1;

        public bool HasNextPage()
        {
            if (filteredMeals == null)
                return false;

            return (currentPage * pageSize) < filteredMeals.Count;
        }
        public int GetCurrentPageNumber()
        {
            return currentPage;
        }

        public int GetTotalPages()
        {
            if (filteredMeals == null || filteredMeals.Count == 0)
                return 1;

            return (int)Math.Ceiling((double)filteredMeals.Count / pageSize);
        }
        public bool HasPreviousPage()
        {
            return currentPage > 1;
        }
        public MealSearchViewModel()
        {
            meals = new List<Meal>()
            {
                new Meal
                {
                    Name = "Chicken Rice",
                    Calories = 500,
                    Protein = 35,
                    Carbs = 50,
                    Fat = 10,
                    IsVegan = false,
                    IsKeto = false,
                    IsGlutenFree = true,
                    IsLactoseFree = true,
                    IsNutFree = true
                },
                new Meal
                {
                    Name = "Beef Burger",
                    Calories = 700,
                    Protein = 40,
                    Carbs = 30,
                    Fat = 35,
                    IsVegan = false,
                    IsKeto = true,
                    IsGlutenFree = false,
                    IsLactoseFree = true,
                    IsNutFree = true
                },
                new Meal
                {
                    Name = "Salad",
                    Calories = 200,
                    Protein = 10,
                    Carbs = 15,
                    Fat = 5,
                    IsVegan = true,
                    IsKeto = true,
                    IsGlutenFree = true,
                    IsLactoseFree = true,
                    IsNutFree = true
                },
                new Meal
                {
                    Name = "Pasta",
                    Calories = 600,
                    Protein = 15,
                    Carbs = 70,
                    Fat = 12,
                    IsVegan = false,
                    IsKeto = false,
                    IsGlutenFree = false,
                    IsLactoseFree = false,
                    IsNutFree = true
                }
            };
        }

        // 🔍 SEARCH + FILTER + RESET PAGE
        public List<Meal> SearchMeals(string query, bool vegan, bool keto, bool gluten, bool lactose, bool nuts, bool favorites)
        {
            filteredMeals = meals
                .Where(m => m.Name.ToLower().Contains(query.ToLower()))
                .Where(m => !vegan || m.IsVegan)
                .Where(m => !keto || m.IsKeto)
                .Where(m => !gluten || m.IsGlutenFree)
                .Where(m => !lactose || m.IsLactoseFree)
                .Where(m => !nuts || m.IsNutFree)
                .Where(m => !favorites || m.IsFavorite)
                .ToList();

            currentPage = 1;

            return GetCurrentPage();
        }

        // 📄 CURRENT PAGE
        public List<Meal> GetCurrentPage()
        {
            if (filteredMeals == null)
                return new List<Meal>();

            return filteredMeals
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        // ➡️ NEXT
        public List<Meal> NextPage()
        {
            if (filteredMeals == null)
                return new List<Meal>();

            if ((currentPage * pageSize) < filteredMeals.Count)
                currentPage++;

            return GetCurrentPage();
        }

        // ⬅️ PREVIOUS
        public List<Meal> PreviousPage()
        {
            if (filteredMeals == null)
                return new List<Meal>();

            if (currentPage > 1)
                currentPage--;

            return GetCurrentPage();
        }

        // ⭐ FAVORITE
        public void ToggleFavorite(Meal meal)
        {
            if (meal != null)
                meal.IsFavorite = !meal.IsFavorite;
        }
    }
}