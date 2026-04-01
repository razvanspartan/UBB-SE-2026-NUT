using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamNut.Models;
using TeamNut.Repositories;

namespace TeamNut.Services
{
    public class MealService
    {
        private readonly MealRepository _mealRepository;

        public MealService()
        {
            _mealRepository = new MealRepository();
        }

        // Return all meals or apply a simple text filter on the meal name
        public Task<List<Meal>> GetMealsAsync(string? filter = null)
        {
            var meals = _mealRepository.GetMeals() ?? new List<Meal>();

            if (string.IsNullOrWhiteSpace(filter) || filter == "All")
                return Task.FromResult(meals);

            var filtered = meals.Where(m => (m.Name ?? string.Empty).Contains(filter, System.StringComparison.OrdinalIgnoreCase)).ToList();
            return Task.FromResult(filtered);
        }

        public async Task<Meal?> GetByIdAsync(int id)
        {
            return await _mealRepository.GetById(id);
        }

        public async Task<List<Meal>> GetAllAsync()
        {
            var list = await _mealRepository.GetAll();
            return list.ToList();
        }
    }
}
