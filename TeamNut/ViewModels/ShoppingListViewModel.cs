using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

using TeamNut.Models;
using TeamNut.Services;

namespace TeamNut.ViewModels
{

    public partial class ShoppingListViewModel : ObservableObject
    {
        private readonly ShoppingListService _shoppingListService;

        [ObservableProperty]
        private ObservableCollection<ShoppingItem> items = new ObservableCollection<ShoppingItem>();

        public ShoppingListViewModel()
        {
            _shoppingListService = new ShoppingListService();
            _ = LoadItemsAsync();
        }

        public async Task LoadItemsAsync()
        {
            // In a real scenario, use UserSession.UserId. Using 1 to mock.
            var loadedItems = await _shoppingListService.GetShoppingItemsAsync(1);
            
            Items.Clear();
            foreach (var item in loadedItems)
            {
                // Subscribe to PropertyChanged to automatically update standard properties to the DB when checked
                item.PropertyChanged += async (s, e) =>
                {
                    if (e.PropertyName == nameof(ShoppingItem.IsChecked))
                    {
                        await _shoppingListService.UpdateItemAsync((ShoppingItem)s);
                    }
                };
                Items.Add(item);
            }
        }

        [RelayCommand]
        public async Task AddItem(string itemName)
        {
            if (!string.IsNullOrWhiteSpace(itemName))
            {
                var newItem = new ShoppingItem { Name = itemName.Trim(), IsChecked = false, UserId = 1 };
                
                bool success = await _shoppingListService.AddItemAsync(newItem);
                if (success)
                {
                    newItem.PropertyChanged += async (s, e) =>
                    {
                        if (e.PropertyName == nameof(ShoppingItem.IsChecked))
                        {
                            await _shoppingListService.UpdateItemAsync((ShoppingItem)s);
                        }
                    };
                    Items.Add(newItem);
                }
            }
        }

        [RelayCommand]
        public async Task DeleteItem(ShoppingItem item)
        {
            if (item != null && Items.Contains(item))
            {
                bool success = await _shoppingListService.RemoveItemAsync(item);
                if (success)
                {
                    Items.Remove(item);
                }
            }
        }
    }
}
