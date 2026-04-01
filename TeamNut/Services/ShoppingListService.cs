using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamNut.Models;

namespace TeamNut.Services
{
    public class ShoppingListService
    {
        private readonly TeamNut.Repositories.ShoppingListRepository _repository;
        private readonly TeamNut.Repositories.IngredientRepository _ingredientRepository;
        private readonly TeamNut.Repositories.InventoryRepository _inventoryRepository;

        public ShoppingListService()
        {
            _repository = new TeamNut.Repositories.ShoppingListRepository();
            _ingredientRepository = new TeamNut.Repositories.IngredientRepository();
            _inventoryRepository = new TeamNut.Repositories.InventoryRepository();
        }

        public async Task<List<ShoppingItem>> GetShoppingItemsAsync(int userId)
        {
            return await _repository.GetAllByUserId(userId);
        }

        public async Task<ShoppingItem> AddItemAsync(string itemName, int userId)
        {
            try
            {
                int ingredientId = await _ingredientRepository.GetOrCreateIngredientIdAsync(itemName);

                var newItem = new ShoppingItem
                {
                    UserId = userId,
                    IngredientId = ingredientId,
                    IngredientName = itemName,
                    IsChecked = false
                };

                await _repository.Add(newItem);
                return newItem;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> RemoveItemAsync(ShoppingItem item)
        {
            try
            {
                await _repository.Delete(item.Id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateItemAsync(ShoppingItem item)
        {
            try
            {
                await _repository.Update(item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<KeyValuePair<int, string>>> SearchIngredientsAsync(string search)
        {
            return await _ingredientRepository.SearchIngredientsAsync(search);
        }

        public async Task<bool> MoveToPantryAsync(ShoppingItem item)
        {
            try
            {
                await _inventoryRepository.Add(new Inventory
                {
                    UserId = item.UserId,
                    IngredientId = item.IngredientId,
                    QuantityGrams = item.QuantityGrams > 0 ? (int)item.QuantityGrams : 100
                });
                await _repository.Delete(item.Id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> GenerateListAsync(int userId)
        {
            try
            {
                return await _repository.GenerateFromMealPlanAsync(userId);
            }
            catch
            {
                return -1;
            }
        }
    }
}
